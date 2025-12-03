using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(NZWalksDbContext dbContext, IRegionRepository regionRepository, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            // Get data from database - Domain Models
            var regionsDomain = await regionRepository.GetAllAsync();

            // Map Domain Models to DTOs
            //var regionsDto = new List<RegionDto>();

            //regionsDomain.ForEach(region =>
            //{
            //    var regionDto = new RegionDto
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        ImageUrl = region.ImageUrl
            //    };
            //    regionsDto.Add(regionDto);
            //});

            // Map Domain Models to DTOs
            var regionsDto = mapper.Map<List<RegionDto>>(regionsDomain);

            return Ok(regionsDto);
        }

        // Get region by id
        [Route("{id:Guid}")]
        [HttpGet]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var regionDomain = await regionRepository.GetByIdAsync(id);
            // var region = dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    ImageUrl = regionDomain.ImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return Ok(regionDto);
        }

        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CreateRegionDto createRegionDto)
        {
            // Map DTO to Domain Model
            //var regionDomain = new Region
            //{
            //    Code = createRegionDto.Code,
            //    Name = createRegionDto.Name,
            //    ImageUrl = createRegionDto?.ImageUrl
            //};

            var regionDomain = mapper.Map<Region>(createRegionDto);

            regionDomain = await regionRepository.CreateAsync(regionDomain);

            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    ImageUrl = regionDomain.ImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }
        
        [Route("{id:Guid}")]
        [HttpPut]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionDto updateRegionDto)
        {
            // Map DTO to Domain Model

            //var regionDomain = new Region
            //{
            //    Code = updateRegionDto.Code,
            //    Name = updateRegionDto.Name,
            //    ImageUrl = updateRegionDto?.ImageUrl
            //};

            var regionDomain = mapper.Map<Region>(updateRegionDto);

            // Check if region exists
            regionDomain = await regionRepository.UpdateAsync(id, regionDomain);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // Map Domain Model to DTO

            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    ImageUrl = regionDomain.ImageUrl
            //};

            var regionDto = mapper.Map<RegionDto>(regionDomain);

            return Ok(regionDto);
        }

        [Route("{id:Guid}")]
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            // Check if region exists
            var regionDomain = await regionRepository.DeleteAsync(id);

            if (regionDomain == null)
            {
                return NotFound();
            }

            // If you want to return the deleted region info
            // Map Domain Model to DTO
            //var regionDto = new RegionDto
            //{
            //    Id = regionDomain.Id,
            //    Code = regionDomain.Code,
            //    Name = regionDomain.Name,
            //    ImageUrl = regionDomain.ImageUrl
            //};
            var regionDto = mapper.Map<RegionDto>(regionDomain);
            return Ok(regionDto);
        }
    }
}
