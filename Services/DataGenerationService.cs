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

        private async void GenerateSensorsData(object? state)
        {


            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<SmartAgroDbContext>();
                var logs = new List<LogsSensor>();
                var sensors = context.Sensors.ToList();
                foreach (var sensor in sensors)
                {
                    var lastLog = context.LogsSensors.OrderByDescending(x => x.DataAtualizacao).FirstOrDefault(x => x.SensorId == sensor.Id);
                    var random = new Random();

                    var log = new LogsSensor
                    {
                        Id = Guid.NewGuid(),
                        SensorId = sensor.Id,
                        DataAtualizacao = DateTime.Now,
                        TemperaturaAr = GenerateValue(random, lastLog?.TemperaturaAr, -10, 45, -5, 5),
                        UmidadeAr = GenerateValue(random, lastLog?.UmidadeAr, 0, 100, -5, 5),
                        QualidadeAr = random.Next(100, 200),
                        TemperaturaSolo = GenerateValue(random, lastLog?.TemperaturaSolo, -5, 40, -5, 5),
                        UmidadeSolo = GenerateValue(random, lastLog?.UmidadeSolo, 0, 100, -5, 5),
                        PhSolo = GenerateValue(random, lastLog?.PhSolo, 0, 14, -1, 1),
                        Luminosidade = GenerateValue(random, lastLog?.Luminosidade, 0, 130000, -5500, 5500)
                    };

                    logs.Add(log);
                }

                context.LogsSensors.AddRange(logs);
                await context.SaveChangesAsync();

                foreach (var log in logs)
                {
                    CheckIfAnyPropertyIsHarmful(log);
                }

            }

        }

        private static decimal GenerateValue(Random random, decimal? lastValue, decimal minValue, decimal maxValue, int minChange, int maxChange)
        {
            var newValue = (lastValue ?? (minValue + maxValue) / 2) + random.Next(minChange, maxChange);
            return Math.Clamp(newValue, minValue, maxValue);
        }

        bool IsOutOfRange(decimal? value, decimal min, decimal max)
        {
            return value <= min || value >= max;
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

                var notification = new Notificacao()
                {
                    Mensagem = "está fora da faixa ideal.",
                    LogsSensorId = log.Id,
                    DataCriacao = DateTime.Now,
                    TipoNotificacaoId = (int)NotificationType.Danger,
                    UsuarioId = log.Sensor.UsuarioId,
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
