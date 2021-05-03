using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.IRepository;
using System;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{

    public class MarketMasterController : BaseController
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IMapper _mapper;

        public MarketMasterController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        // GET: api/MarketMaster
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<MarketMaster>>> GetMarketMasters()
        //{
        //    return await _context.MarketMasters.ToListAsync();
        //}

        // GET: api/MarketMaster/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<MarketMaster>> GetMarketMaster(int id)
        //{
        //    var marketMaster = await _context.MarketMasters.FindAsync(id);

        //    if (marketMaster == null)
        //    {
        //        return NotFound();
        //    }

        //    return marketMaster;
        //}

        // PUT: api/MarketMaster/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutMarketMaster(int id, MarketMaster marketMaster)
        //{
        //    if (id != marketMaster.Id)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(marketMaster).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!MarketMasterExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}


        [HttpPost]
        public async Task<IActionResult> PostMarketMaster(MarketMasterDto dto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                await _unitOfWork.MarketAllocationRepository.CreateAsync(dto);

                if (await _unitOfWork.SaveChangesAsync())
                    return Ok();
                    //return CreatedAtAction("GetMarketAllocation", new { id = dto.Id }, dto);
                return BadRequest();
            }
            catch (Exception e)
            {
                return ExceptionResponse(e.Message, dto);
            }
        }

        // DELETE: api/MarketMaster/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteMarketMaster(int id)
        //{
        //    var marketMaster = await _context.MarketMasters.FindAsync(id);
        //    if (marketMaster == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.MarketMasters.Remove(marketMaster);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        //private bool MarketMasterExists(int id)
        //{
        //    return _context.MarketMasters.Any(e => e.Id == id);
        //}
    }
}
