using SmartAgroAPI.Contexts;
using SmartAgroAPI.Models;
using static SmartAgroAPI.Enums.Enums;

namespace SmartAgroAPI.Services
{
    public class DataGenerationService : IHostedService
    {

        private readonly IServiceProvider _serviceProvider;
        private Timer? timer;


        public DataGenerationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private void GenerateSensorsData(object? state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SmartAgroDbContext>();

                var sensors = context.Sensors.ToList();
                foreach (var sensor in sensors)
                {
                    var log = new LogsSensor()
                    {
                        SensorId = sensor.Id,
                        DataAtualizacao = DateTime.Now
                    };

                    var r = new Random();

                    var lastlogFromSensor = context.LogsSensors.OrderByDescending(x => x.DataAtualizacao).FirstOrDefault();
                    log.TemperaturaAr = Math.Clamp(lastlogFromSensor!.TemperaturaAr!.Value + r.Next(-5, 5), -10, 45);
                    log.UmidadeAr = Math.Clamp(lastlogFromSensor.UmidadeAr!.Value + r.Next(-5, 5), 0, 100);
                    log.QualidadeAr = r.Next(100, 200);
                    log.TemperaturaSolo = Math.Clamp(lastlogFromSensor.TemperaturaSolo!.Value + r.Next(-5, 5), -5, 40);
                    log.UmidadeSolo = Math.Clamp(lastlogFromSensor.UmidadeSolo!.Value + r.Next(-5, 5), 0, 100);
                    log.PhSolo = Math.Clamp(lastlogFromSensor.PhSolo!.Value + r.Next(-1, 1), 0, 14);
                    log.Luminosidade = Math.Clamp(lastlogFromSensor.Luminosidade!.Value + r.Next(-5500, 5500), 0, 130000);

                    context.LogsSensors.Add(log);

                    CheckIfAnyPropertyIsHarmful(log);
                }

                context.SaveChanges();
            }

        }

        bool IsOutOfRange(decimal? value, decimal min, decimal max)
        {
            return value <= min && value >= max;
        }

        private void CheckIfAnyPropertyIsHarmful(LogsSensor log)
        {

            if (IsOutOfRange(log.Luminosidade, 1000, 100000))
            {
                Notify(log, nameof(log.Luminosidade));
                return;
            }
            if (IsOutOfRange(log.TemperaturaAr, 5, 25))
            {
                Notify(log, nameof(log.TemperaturaAr));
                return;
            }
            if (IsOutOfRange(log.TemperaturaSolo, 5, 20))
            {
                Notify(log, nameof(log.Luminosidade));
                return;
            }
            if (IsOutOfRange(log.UmidadeSolo, 40, 85))
            {
                Notify(log, nameof(log.UmidadeSolo));
                return;
            }
            if (IsOutOfRange(log.UmidadeAr, 30, 80))
            {
                Notify(log, nameof(log.UmidadeAr));
                return;
            }
            if (IsOutOfRange(log.PhSolo, 5, 12))
            {
                Notify(log, nameof(log.PhSolo));
                return;
            }
            if (IsOutOfRange(log.QualidadeAr, 100, 200))
            {
                Notify(log, nameof(log.QualidadeAr));
                return;
            }
        }

        private void Notify(LogsSensor log, string v)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SmartAgroDbContext>();

                if (context.Notificacaos.Any(x => x.Propriedade == "v" && x.DataCriacao!.Value.Date == DateTime.Now.Date))
                {
                    return;
                }
                var sensor = context.LogsSensors.FirstOrDefault(x => x.DataAtualizacao == log.DataAtualizacao)!;
                var notification = new Notificacao()
                {
                    Mensagem = "está fora da faixa ideal.",
                    LogsSensorId = sensor.Id,
                    DataCriacao = DateTime.Now,
                    TipoNotificacaoId = (int)NotificationType.Danger,
                    UsuarioId = sensor!.Sensor!.UsuarioId,
                    Propriedade = v

                };
                context.Notificacaos.Add(notification);

                context.SaveChanges();
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(GenerateSensorsData, null, TimeSpan.Zero, TimeSpan.FromMinutes(15));
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
