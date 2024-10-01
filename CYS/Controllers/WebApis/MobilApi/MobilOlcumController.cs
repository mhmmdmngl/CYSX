﻿using Microsoft.AspNetCore.Mvc;
using CYS.Models;
using CYS.Repos;

namespace CYS.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class MobilOlcumController : ControllerBase
	{
		private readonly MobilOlcumCTX _mobilOlcumCTX;

		public MobilOlcumController()
		{
			_mobilOlcumCTX = new MobilOlcumCTX();
		}

		// GET: api/MobilOlcum
		[HttpGet]
		public IActionResult GetAll()
		{
			var result = _mobilOlcumCTX.MobilOlcumList("SELECT * FROM mobilolcum", null);
			return Ok(result);
		}

		// GET: api/MobilOlcum/5
		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var result = _mobilOlcumCTX.MobilOlcumTek("SELECT * FROM mobilolcum WHERE Id = @Id", new { Id = id });
			if (result == null)
			{
				return NotFound();
			}
			return Ok(result);
		}

		// POST: api/MobilOlcum
		[HttpPost]
		public IActionResult Create([FromBody] MobilOlcum mobilOlcum)
		{
			if (mobilOlcum == null)
			{
				return BadRequest();
			}
			_mobilOlcumCTX.MobilOlcumEkle(mobilOlcum);
			return CreatedAtAction(nameof(GetById), new { id = mobilOlcum.Id }, mobilOlcum);
		}

		// PUT: api/MobilOlcum/5
		[HttpPut("{id}")]
		public IActionResult Update(int id, [FromBody] MobilOlcum mobilOlcum)
		{
			if (mobilOlcum == null || mobilOlcum.Id != id)
			{
				return BadRequest();
			}

			var existingMobilOlcum = _mobilOlcumCTX.MobilOlcumTek("SELECT * FROM mobilolcum WHERE Id = @Id", new { Id = id });
			if (existingMobilOlcum == null)
			{
				return NotFound();
			}

			_mobilOlcumCTX.MobilOlcumGuncelle(mobilOlcum);
			return NoContent();
		}

		// DELETE: api/MobilOlcum/5
		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			var existingMobilOlcum = _mobilOlcumCTX.MobilOlcumTek("SELECT * FROM mobilolcum WHERE Id = @Id", new { Id = id });
			if (existingMobilOlcum == null)
			{
				return NotFound();
			}

			// SİLME İŞLEMİ OLUŞTURULMADI, isteğe göre eklenebilir.
			return NoContent();
		}
	}
}
