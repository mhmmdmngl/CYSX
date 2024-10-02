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

        [HttpPost]
        public IActionResult Create([FromBody] MobilOlcum mobilOlcum)
        {
            if (mobilOlcum == null)
            {
                return BadRequest("MobilOlcum can't be null");
            }

            if (string.IsNullOrEmpty(mobilOlcum.Rfid))
            {
                return BadRequest("RFID is required");
            }

            if (mobilOlcum.Weight <= 0)
            {
                return BadRequest("Weight must be greater than 0");
            }

            //mobilOlcum.Tarih = DateTime.Now; // Eğer tarih gönderilmemişse
			if (mobilOlcum.AmacId == 1)
				mobilOlcum.Amac = "Normal Tartım";
			else if (mobilOlcum.AmacId == 2)
				mobilOlcum.Amac = "İlk Doğum Ağırlığı";
            else if (mobilOlcum.AmacId == 3)
                mobilOlcum.Amac = "Sütten Kesim";
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

			if (mobilOlcum.AmacId == 1)
				mobilOlcum.Amac = "Normal Tartım";
			else if (mobilOlcum.AmacId == 2)
				mobilOlcum.Amac = "İlk Doğum Ağırlığı";
            else if (mobilOlcum.AmacId == 3)
                mobilOlcum.Amac = "Sütten Kesim";


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

        [HttpGet("GetLast20")]
        public IActionResult GetLast20MobilOlcum()
        {
            MobilOlcumCTX ctx = new MobilOlcumCTX();
            var data = ctx.MobilOlcumList("SELECT * FROM mobilolcum ORDER BY tarih DESC LIMIT 15", null);
			HayvanCTX hctx = new HayvanCTX();
			foreach(var item in data)
			{
				item.hayvan = hctx.hayvanTek("select * from Hayvan where rfidKodu = @rf", new {rf = item.Rfid});
                if (item.AmacId == 1)
                    item.Amac = "Normal Tartım";
                else if (item.AmacId == 2)
                    item.Amac = "İlk Doğum Ağırlığı";
                else if (item.AmacId == 3)
                    item.Amac = "Sütten Kesim";
            }
            return Ok(data);
        }
    }
}
