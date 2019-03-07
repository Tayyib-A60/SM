using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SkineroMotors.Controllers.Resources;
using SkineroMotors.Core;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Controllers {
    [Route ("/api/skineroVehicles")]
    [ApiController]
    public class VehicleController : Controller {
        private IMapper _mapper { get; }
        private IVehicleRepository _repository { get; }
        private IPhotoRepository _photoRepository { get; }
        private IHostingEnvironment _host { get; }
        private IUnitOfWork _unitOfWork { get; }
        public VehicleController (IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork,IPhotoRepository photoRepository, IHostingEnvironment host)
        {
            this._unitOfWork = unitOfWork;
            this._repository = repository;
            this._mapper = mapper;
            this._photoRepository = photoRepository;
            this._host = host;
        }
        [HttpPost]
        public async Task<IActionResult> CreateVehicle(VehicleResource vehicleResource) 
        {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            var vehicle = _mapper.Map<VehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;
            _repository.Add(vehicle);

            await _unitOfWork.CompleteAsync();
            vehicle = await _repository.GetVehicle(vehicle.Id);
            return Ok(vehicle);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVehicle(int id, SaveVehicleResource vehicleResource)
        {
            if (!ModelState.IsValid)
                return BadRequest (ModelState);
            var vehicle = await _repository.GetVehicle(id);
            if (vehicle == null)
                return NotFound("Not Found");
            
            _mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;
            await _unitOfWork.CompleteAsync();

            vehicle = await _repository.GetVehicle(vehicle.Id);
            var result = _mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVehicle (int id)
        {
            var  vehicle = await _repository.GetVehicle(id);
            if (vehicle == null)
                return NotFound ("Not found");

            var photos = await _photoRepository.GetPhotos(vehicle.Id);
            if(photos == null)
                return NotFound("Photo not found");
            
            var uploadsFolderPath = Path.Combine (_host.WebRootPath, "uploads");
            foreach (Photo p in photos) {
            var filePath = Path.Combine (uploadsFolderPath, p.FileName);
            FileInfo fileInfo = new FileInfo(filePath);
            if(fileInfo.Exists)
                fileInfo.Delete();
            }
            
            _repository.Remove(vehicle);
            await _unitOfWork.CompleteAsync();

            return Ok(id);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await _repository.GetVehicle(id);
            if (vehicle == null)
                return NotFound("Vehicle Not found");
            var vehicleResource = _mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(vehicleResource);
        }
        [HttpGet]
        public async Task<QueryResultResource<VehicleResource>> GetVehicles([FromQuery] VehicleQueryResource vehicleQueryResource, string searchString)
        {
            var vehicle = _mapper.Map<VehicleQueryResource, VehicleQuery>(vehicleQueryResource);
            var result = await _repository.GetVehicles(vehicle, searchString);
            return  _mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(result);
        }
        [HttpGet("featured")]
        public async Task<QueryResultResource<VehicleResource>> GetFeaturedVehicles([FromQuery] VehicleQueryResource vehicleQueryResource)
        {
            var vehicle = _mapper.Map<VehicleQueryResource, VehicleQuery>(vehicleQueryResource);
            var result = await _repository.GetFeaturedVehicles(vehicle);

            return  _mapper.Map<QueryResult<Vehicle>, QueryResultResource<VehicleResource>>(result);
        }
    }
}