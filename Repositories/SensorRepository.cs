using AutoMapper;
using SmartAgroAPI.Contexts;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Interfaces;
using SmartAgroAPI.Models;

namespace SmartAgroAPI.Repositories
{
    public class SensorRepository : ISensorRepository
    {

        public readonly SmartAgroDbContext _context;
        public readonly IMapper _mapper;

        public SensorRepository(SmartAgroDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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

        public void Edit(int sensorId, EditSensorDTO editedSensor)
        {
            var oldSensor = GetById(sensorId);
            _mapper.Map(editedSensor, oldSensor);
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
                    SensorLogs = x.LogsSensors.Select(x => new LogsSensorDTO(x)).Where(x => x.DataAtualizacao.Value > DateTime.Now.AddDays(-7)).ToList(),
                })
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
