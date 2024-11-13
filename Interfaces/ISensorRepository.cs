using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Models;

namespace SmartAgroAPI.Interfaces
{
    public interface ISensorRepository
    {
        List<LogsSensorDTO> GetLogs(int sensorId);
        List<SensorDTO> GetSensors(Guid userId);
        void Edit(int sensorId, Sensor editedSensor);
        void Register(Sensor sensor);
        void ChangeLocation(int sensorId, decimal longitude, decimal latitude);
        void CreateLog(LogsSensor log);

        Sensor? GetById(int sensorId);


        //Data Generation dedicated functions
        void GenerateDataForAllSensors();
        void GenerateDataForSensor(int sensorId);

    }
}
