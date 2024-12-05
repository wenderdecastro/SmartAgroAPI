using Microsoft.AspNetCore.Mvc;
using SmartAgroAPI.DataTransferObjects;
using SmartAgroAPI.Interfaces;

namespace SmartAgroAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SensorController : ControllerBase
    {
        private readonly ISensorRepository _sensorRepository;
        private readonly IUserRepository _userRepository;

        public SensorController(ISensorRepository sensorRepository, IUserRepository userRepository)
        {
            _sensorRepository = sensorRepository;
            _userRepository = userRepository;
        }


        //GET api/Sensor/{id}
        [HttpGet("{sensorId}")]
        public IActionResult GetAllLogsFromASensor(int sensorId)
        {
            var logs = _sensorRepository.GetLogs(sensorId);
            return Ok(logs);
        }

        //GET api/Sensor/User/{id}
        [HttpGet("user/{userId}")]
        public IActionResult GetAllSensorsFromAUser(Guid userId)
        {
            if (_userRepository.GetById(userId) == null) return NotFound("User not Found");
            var response = _sensorRepository.GetSensors(userId);

            return Ok(response);
        }


        //PATCH api/Sensor/{id}
        [HttpPatch("{sensorId}")]
        public IActionResult EditSensor(int sensorId, [FromBody] EditSensorDTO editedSensor)
        {
            if (_sensorRepository.GetById(sensorId) == null) return NotFound("Sensor doesn't exist");
            _sensorRepository.Edit(sensorId, editedSensor);
            return NoContent();

        }

        //POST api/Sensor

        [HttpPost]
        public IActionResult RegisterSensor([FromBody] EditSensorDTO sensor)
        {
            _sensorRepository.Register(sensor);
            return Created();
        }



    }
}
