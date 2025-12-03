using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.CustomActionFilters;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IMapper mapper;
        private readonly IWalkRepository walkRepository;

        public WalksController(IMapper mapper, IWalkRepository walkRepository)
        {
            this.mapper = mapper;
            this.walkRepository = walkRepository;
        }

        // CREATE Walk
        // POST: /api/walks
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CreateWalkDto createWalkDto)
        {
            // Map DTO to Domain Model
            var walkDomain = mapper.Map<Walk>(createWalkDto);
            await walkRepository.CreateAsync(walkDomain);

            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        // GET Walks
        // GET: /api/walks
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var walksDomain = await walkRepository.GetAllAsync();

            //Map Domain Model to DTO
            return Ok(mapper.Map<List<WalkDto>>(walksDomain));
        }

        // GET Walk by Id
        // GET: /api/walks/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            var walkDomain = await walkRepository.GetByIdAsync(id);
            if (walkDomain == null)
            {
                return NotFound();
            }
            //Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        // UPDATE Walk By Id
        [HttpPut]
        [Route("{id:Guid}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateWalkDto updateWalkDto)
        {
            // Map DTO to Domain Model
            var walkDomain = mapper.Map<Walk>(updateWalkDto);
            walkDomain = await walkRepository.UpdateAsync(id, walkDomain);
            if (walkDomain == null)
            {
                return NotFound();
            }
            // Map Domain Model to DTO
            return Ok(mapper.Map<WalkDto>(walkDomain));
        }

        // DELETE Walk By Id
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var deletedWalkDomain = await walkRepository.DeleteAsync(id);
            if (deletedWalkDomain == null)
            {
                return NotFound();
            }
            return Ok(mapper.Map<WalkDto>(deletedWalkDomain));
        }
    }
}
