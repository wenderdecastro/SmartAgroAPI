using SmartAgroAPI.Contexts;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Interfaces;
using SmartAgroAPI.Models;

namespace SmartAgroAPI.Repositories
{
    public class SensorRepository : ISensorRepository
    {

        public readonly SmartAgroDbContext _context;

        public SensorRepository()
        {
            _context = new SmartAgroDbContext();
        }

        public void ChangeLocation(int sensorId, decimal longitude, decimal latitude)
        {
            var sensor = GetById(sensorId);
            sensor!.Latitude = latitude;
            sensor!.Longitude = longitude;
            _context.SaveChanges();
        }

        public void CreateLog(LogsSensor log)
        {
            _context.LogsSensors.Add(log);
            _context.SaveChanges();
        }

        public void Edit(int sensorId, Sensor editedSensor)
        {
            var oldSensor = GetById(sensorId);

            foreach (var property in typeof(Sensor).GetProperties())
            {
                if (property.Name == "Id") continue;
                var oldValue = property.GetValue(oldSensor);
                var newValue = property.GetValue(editedSensor);

                if (!Equals(oldValue, newValue))
                {
                    property.SetValue(oldSensor, newValue);
                }
            }

            _context.Sensors.Update(oldSensor!);
            _context.SaveChanges();
        }

        public void GenerateDataForAllSensors()
        {
            throw new NotImplementedException();
        }

        public void GenerateDataForSensor(int sensorId)
        {
            throw new NotImplementedException();
        }

        public Sensor? GetById(int sensorId) => _context.Sensors.Find(sensorId);

        public List<LogsSensorDTO> GetLogs(int sensorId)
        {
            List<LogsSensorDTO> logs = _context.LogsSensors.Where(x => x.SensorId == sensorId)
                .Select(x => new LogsSensorDTO(x))
                .OrderByDescending(x => x.DataAtualizacao)
                .ToList();

            return logs;
        }

        public List<SensorDTO> GetSensors(Guid userId)
        {
            var sensores = _context.Sensors.Where(x => x.UsuarioId == userId)
                .Select(x =>
                new SensorDTO(x)
                {
                    SensorLogs = x.LogsSensors.Select(x => new LogsSensorDTO(x)).ToList()
                }

                )
                .ToList();

            return sensores;

        }

        public void Register(Sensor sensor)
        {
            _context.Sensors.Add(sensor);
            _context.SaveChanges();
        }
    }
}
