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
            vaccine.Name = updateVaccine.Name ?? vaccine.Name;
            //vaccine.VaccineTypeID = updateVaccine.VaccineTypeID ?? vaccine.VaccineTypeID;
            vaccine.Manufacturer = updateVaccine.Manufacturer ?? vaccine.Manufacturer;
            vaccine.Stock = updateVaccine.Stock ?? vaccine.Stock;
            vaccine.Price = updateVaccine.Price ?? vaccine.Price;
            vaccine.Description = updateVaccine.Description ?? vaccine.Description;
            vaccine.FromAge = updateVaccine.FromAge ?? vaccine.FromAge;
            vaccine.ToAge = updateVaccine.ToAge ?? vaccine.ToAge;
            vaccine.Period = updateVaccine.Period ?? vaccine.Period;
            vaccine.DosesRequired = updateVaccine.DosesRequired ?? vaccine.DosesRequired;
            vaccine.Priority = updateVaccine.Priority ?? vaccine.Priority;

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
