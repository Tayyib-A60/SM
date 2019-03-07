using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using SkineroMotors.Controllers.Resources;
using SkineroMotors.Core;
using SkineroMotors.Core.Models;

namespace SkineroMotors.Controllers {
    [Route ("/api/skineroVehicles/photos/{vehicleId}")]
    [ApiController]
    public class PhotosController : Controller {
        private PhotoSettings _photoSettings { get; }
        private IHostingEnvironment _host { get; }
        private IMapper _mapper { get; }
        private IUnitOfWork _unitOfWork { get; }
        private IVehicleRepository _vehicleRepository { get; }
        private IPhotoRepository _photoRepository { get; }
        public PhotosController (IHostingEnvironment host, IMapper mapper, IUnitOfWork unitOfWork, IVehicleRepository vehicleRepository, IPhotoRepository photoRepository, IOptionsSnapshot<PhotoSettings> options)
        {
            _photoSettings = options.Value;
            _photoRepository = photoRepository;
            _vehicleRepository = vehicleRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _host = host;
        }
        [HttpGet]
        public async Task<IEnumerable<PhotoResource>> GetVehicles(int vehicleId)
        {
            var photos = await _photoRepository.GetPhotos(vehicleId);
            return _mapper.Map<IEnumerable<Photo>, IEnumerable<PhotoResource>>(photos);
        //     var a = new List<int> {};
        //     a.Add(1);
        //     int[] b = new int[] {};
        //     var x = b.Length;
        }
        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
             var vehicle = await _vehicleRepository.GetVehicle (vehicleId);
            if (vehicle == null) return NotFound ("Not Found");
            if (file == null) return BadRequest ("Null file");
            if (file.Length == 0) return BadRequest ("Empty file");
            if (file.Length > _photoSettings.MaxBytes) return BadRequest ("Maximum file size exceeded");
            if (!_photoSettings.isSupported (file.FileName)) return BadRequest ("Invalid file type.");

             var uploadsFolderPath = Path.Combine (_host.WebRootPath, "uploads");
            if (!Directory.Exists (uploadsFolderPath))
                Directory.CreateDirectory (uploadsFolderPath);

            var fileName = Guid.NewGuid ().ToString () + Path.GetExtension (file.FileName);
            var filePath = Path.Combine (uploadsFolderPath, fileName);

            using (var stream = new FileStream (filePath, FileMode.Create)) {
                await file.CopyToAsync (stream);
            }
            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add (photo);
            await _unitOfWork.CompleteAsync ();
            return Ok (_mapper.Map<Photo, PhotoResource> (photo));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int id, int vehicleId)
        {
            var photo = await _photoRepository.GetPhoto(id, vehicleId);
            if(photo == null)
                return NotFound("Photo not found");
            var uploadsFolderPath = Path.Combine (_host.WebRootPath, "uploads");
            var filePath = Path.Combine (uploadsFolderPath, photo.FileName);
            FileInfo fileInfo = new FileInfo(filePath);
            _photoRepository.Remove(photo);
            if(fileInfo.Exists)
                fileInfo.Delete();
            await _unitOfWork.CompleteAsync();

            // var ret = $"Photo with id {id} and vehicleId {vehicleId} succesfully removed"; 

            return Ok(id);
        }
    }
}