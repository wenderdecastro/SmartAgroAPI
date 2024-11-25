using SmartAgroAPI.Contexts;
using SmartAgroAPI.Models;

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

                    log.TemperaturaAr = sensor.TemperaturaArIdeal!.Value + r.Next(-5, 5);
                    log.UmidadeAr = sensor.UmidadeArIdeal!.Value + r.Next(-5, 5);
                    log.QualidadeAr = r.Next(100, 200);
                    log.TemperaturaSolo = sensor.TemperaturaSoloIdeal!.Value + r.Next(-5, 5);
                    log.UmidadeSolo = sensor.UmidadeSoloIdeal!.Value + r.Next(-5, 5);
                    log.PhSolo = sensor.PhSoloIdeal!.Value + r.Next(-3, 3);
                    log.Luminosidade = sensor.LuminosidadeIdeal!.Value + r.Next(-5000, 5000);

                    context.LogsSensors.Add(log);
                }

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
