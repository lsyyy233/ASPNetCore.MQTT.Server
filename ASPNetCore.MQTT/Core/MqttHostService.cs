using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MQTTnet;
using MQTTnet.Protocol;
using MQTTnet.Server;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ASPNetCore.MQTT
{
	public class MqttHostService : IHostedService
	{
		private IMqttServer mqttServer;
		private readonly IConfiguration configuration;
		private readonly ILogger log;

		public MqttHostService(IConfiguration configuration, ILoggerFactory loggerFactory, CustomerMqttService customerMqttService)
		{
			this.configuration = configuration;
			mqttServer = customerMqttService.GetMqttServer();
			log = loggerFactory.CreateLogger<MqttHostService>();
		}

		public async Task StartAsync(CancellationToken cancellationToken)
		{
			MqttServerOptions options = new MqttServerOptions
			{
				//连接验证
				ConnectionValidator = new MqttServerConnectionValidatorDelegate(p =>
				{
					if (p.Username != configuration.GetValue<string>("MQTTOption:UserName")
					    || p.Password != configuration.GetValue<string>("MQTTOption:Password"))
					{
						p.ReasonCode = MqttConnectReasonCode.BadUserNameOrPassword;
					}
				})
			};
			options.DefaultEndpointOptions.Port = configuration.GetValue<int>("MQTTOption:Port");
			//创建Mqtt服务器
			mqttServer.ClientSubscribedTopicHandler =
				new MqttServerClientSubscribedHandlerDelegate(MqttNetServer_SubscribedTopic);//开启订阅事件
			mqttServer.ClientUnsubscribedTopicHandler =
				new MqttServerClientUnsubscribedTopicHandlerDelegate(MqttNetServer_UnSubscribedTopic);//取消订阅事件
			mqttServer
				.UseApplicationMessageReceivedHandler(MqttServe_ApplicationMessageReceived)//客户端消息事件
				.UseClientConnectedHandler(MqttNetServer_ClientConnected)//客户端连接事件
				.UseClientDisconnectedHandler(MqttNetServer_ClientDisConnected);//客户端断开事件
			await mqttServer.StartAsync(options);//启动服务
			log.LogInformation($"MQTT service listening ON {configuration.GetValue<int>("MQTTOption:Port")}");
		}

		public Task StopAsync(CancellationToken cancellationToken)
		{
			mqttServer.Dispose();
			return Task.CompletedTask;
		}

		/// <summary>
		/// 客户订阅
		/// </summary>
		private void MqttNetServer_SubscribedTopic(MqttServerClientSubscribedTopicEventArgs e)
		{
			//客户端Id
			string ClientId = e.ClientId;
			string Topic = e.TopicFilter.Topic;
			log.LogInformation($"client [{ClientId}] Subscribed Topic [{Topic}]");
		}

		/// <summary>
		/// 客户取消订阅
		/// </summary>
		private void MqttNetServer_UnSubscribedTopic(MqttServerClientUnsubscribedTopicEventArgs e)
		{
			//客户端Id
			string ClientId = e.ClientId;
			string Topic = e.TopicFilter;
			log.LogInformation($"client [{ClientId}] UnSubscribed Topic [{Topic}]");
		}

		/// <summary>
		/// 接收消息
		/// </summary>
		private void MqttServe_ApplicationMessageReceived(MqttApplicationMessageReceivedEventArgs e)
		{
			string ClientId = e.ClientId;
			string Topic = e.ApplicationMessage.Topic;
			string Payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
			MqttQualityOfServiceLevel Qos = e.ApplicationMessage.QualityOfServiceLevel;
			var Retain = e.ApplicationMessage.Retain;
			log.LogInformation($"client [{ClientId}] pushed topic [{Topic}] payload [{Payload}] Qos [{Qos}] Retain [{Retain}]");
		}

		/// <summary>
		/// 客户连接
		/// </summary>
		private void MqttNetServer_ClientConnected(MqttServerClientConnectedEventArgs e)
		{
			string ClientId = e.ClientId;
			log.LogInformation($"client [{ClientId}] connected");
		}

		/// <summary>
		/// 客户连接断开
		/// </summary>
		private void MqttNetServer_ClientDisConnected(MqttServerClientDisconnectedEventArgs e)
		{
			string ClientId = e.ClientId;
			log.LogInformation($"client [{ClientId}] disconnected");
		}
	}
}
