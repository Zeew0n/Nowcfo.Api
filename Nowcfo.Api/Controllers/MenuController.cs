﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Nowcfo.API.Controllers.Base;
using Nowcfo.Application.Dtos;
using Nowcfo.Application.IRepository;
using Nowcfo.Application.Services.CurrentUserService;
using Nowcfo.Domain.Models;
using Nowcfo.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nowcfo.API.Controllers
{

    public class MenuController : BaseController
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public MenuController(ApplicationDbContext context,IMapper mapper,IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _context = context;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        // GET: api/Menus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Menu>>> GetMenus()
        {
            return await _context.Menus.ToListAsync();
        }

        // GET: api/Menus/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Menu>> GetMenu(Guid id)
        {
            var menu = await _context.Menus.FindAsync(id);

            if (menu == null)
            {
                return NotFound();
            }

            return menu;
        }

        // PUT: api/Menus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMenu(Guid id, Menu menu)
        {
            if (id != menu.Id)
            {
                return BadRequest();
            }

            _context.Entry(menu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MenuExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Menus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Menu>> PostMenu(Menu menu)
        {
            _context.Menus.Add(menu);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMenu", new { id = menu.Id }, menu);
        }

        // DELETE: api/Menus/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMenu(Guid id)
        {
            var menu = await _context.Menus.FindAsync(id);
            if (menu == null)
            {
                return NotFound();
            }

            _context.Menus.Remove(menu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool MenuExists(Guid id)
        {
            return _context.Menus.Any(e => e.Id == id);
        }

        [HttpGet("ChildMenus/{parentMenuId}")]
        public async Task<IActionResult> GetChildMenus(Guid parentMenuId)
        {
            var childMenus = await _context.Menus.Where(x => x.UnderMenuId == parentMenuId).OrderBy(x=>x.DisplayOrder).ToListAsync();
            return Ok(_mapper.Map<List<MenuDto>>(childMenus));
        }

        [HttpGet("AssignedMenus")]
        public async Task<IActionResult> GetMenusAssignedByRoles()
        {
            var userId = _currentUserService.GetUserId();
            var menus = await _unitOfWork.MenuRepository.GetMenusByUserRoleAsync(userId);
            return Ok(menus);
        }
    }
}
