using AutoMapper;
using VaccineScheduleTracking.API.Models.DTOs;
using VaccineScheduleTracking.API.Models.Entities;
using VaccineScheduleTracking.API.Repository;

namespace VaccineScheduleTracking.API.Services
{
    public class VaccineService : IVaccineService
    {
        private readonly IVaccineRepository vaccineRepository;
        private readonly IMapper mapper;

        public VaccineService(IVaccineRepository vaccineRepository, IMapper mapper)
        {
            this.vaccineRepository = vaccineRepository;
            this.mapper = mapper;
        }

        // Vaccine funtion
        public async Task<List<Vaccine>> GetVaccinesAsync(FilterVaccineDto filterVaccineDto)
        {
            return await vaccineRepository.GetVaccinesAsync(filterVaccineDto);
        }

        public async Task<Vaccine?> CreateVaccineAsync(AddVaccineDto addVaccineDto)
        {
            var vaccine = await vaccineRepository.GetVaccineByNameAsync(addVaccineDto.Name);
            if (vaccine != null)
            {
                throw new Exception($"{addVaccineDto.Name} is exist!");
            }
            var vaccineType = await vaccineRepository.GetVaccineTypeByNameAsync(addVaccineDto.VaccineType);
            if (vaccineType == null)
            {
                throw new Exception($"{addVaccineDto.VaccineType} is invalid!");
            }
            vaccine = new Vaccine
            {
                Name = addVaccineDto.Name,
                VaccineTypeID = vaccineType.VaccineTypeID,
                Manufacturer = addVaccineDto.Manufacturer,
                Stock = addVaccineDto.Stock,
                Price = addVaccineDto.Price,
                Description = addVaccineDto.Description,
                FromAge = addVaccineDto.FromAge,
                ToAge = addVaccineDto.ToAge,
                Period = addVaccineDto.Period,
                VaccineType = vaccineType
            };
            await vaccineRepository.AddVaccineAsync(vaccine);
            return vaccine;
        }
        public async Task<Vaccine?> UpdateVaccineAsync(int id, UpdateVaccineDto updateVaccine)
        {
            var vaccine = await vaccineRepository.GetVaccineByIDAsync(id);
            if (vaccine == null)
            {
                throw new Exception($"Can't find vaccine with ID {id}");
            }
            
            vaccine.Name = updateVaccine.Name == "string" ? vaccine.Name : updateVaccine.Name;
            //vaccine.VaccineTypeID = updateVaccine.VaccineTypeID ?? vaccine.VaccineTypeID;
            vaccine.Manufacturer = updateVaccine.Manufacturer == "string" ? vaccine.Manufacturer : updateVaccine.Manufacturer;
            vaccine.Stock = (updateVaccine.Stock == 0 ? vaccine.Stock : updateVaccine.Stock);
            vaccine.Price = updateVaccine.Price == 0 ? vaccine.Price : updateVaccine.Price;
            vaccine.Description = updateVaccine.Description == "string" ? vaccine.Description : updateVaccine.Description;
            vaccine.FromAge = updateVaccine.FromAge == 0 ? vaccine.FromAge : updateVaccine.FromAge;
            vaccine.ToAge = updateVaccine.ToAge == 0 ? vaccine.ToAge : updateVaccine.ToAge;
            vaccine.Period = updateVaccine.Period == 0 ? vaccine.Period : updateVaccine.Period;
            vaccine.DosesRequired = updateVaccine.DosesRequired == 0 ? vaccine.DosesRequired : updateVaccine.DosesRequired;
            vaccine.Priority = updateVaccine.Priority == 0 ? vaccine.Priority : updateVaccine.Priority;

            return await vaccineRepository.UpdateVaccineAsync(vaccine);
        }

        public async Task<Vaccine?> DeleteVaccineAsync(int id)
        {
            var vaccine = await vaccineRepository.GetVaccineByIDAsync(id);
            if (vaccine == null)
            {
                throw new Exception($"VaccineID {id} not found!");
            }
            return await vaccineRepository.DeleteVaccineAsync(vaccine);
        }

        // VaccineType function
        public async Task<VaccineType?> CreateVaccineTypeAsync(AddVaccineTypeDto addVaccineTypeDto)
        {
            var vaccineType = await vaccineRepository.GetVaccineTypeByNameAsync(addVaccineTypeDto.Name);

            if (vaccineType != null)
            {
                return null;
            }

            vaccineType = await vaccineRepository.AddVaccineTypeAsync(mapper.Map<VaccineType>(addVaccineTypeDto));

            return vaccineType;
        }




    }
}
