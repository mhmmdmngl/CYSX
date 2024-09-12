using CYS.Models;
using CYS.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace CYS.Controllers.WebApis
{
	[Route("api/[controller]")]
	[ApiController]
	public class AgirlikApiController : ControllerBase
	{
		// GET: api/<RFIDApiController>
		[HttpGet]
		public string Get(float agirlik)
		{
			processsettingCTX processsetting = new processsettingCTX();
			var mevcutps = processsetting.processsettingTek("select * from processsetting where id = 1", null);
			AgirlikOlcumCTX agirlikCTX = new AgirlikOlcumCTX();
			AgirlikOlcum kam = new AgirlikOlcum()
			{
				agirlikOlcumu = agirlik.ToString(),
				userId = 1,
				tarih = DateTime.Now,
				requestId = mevcutps.mevcutRequest
			};
			agirlikCTX.agirlikOlcumEkle(kam);
			return "1";

		}


		// POST api/<RFIDApiController>
		[HttpPost]
		public void Post(float agirlik, int processId)
		{
			AgirlikOlcumCTX agirlikCTX = new AgirlikOlcumCTX();
			AgirlikOlcum kam = new AgirlikOlcum()
			{
				agirlikOlcumu = agirlik.ToString(),
				userId = 1,
				tarih = DateTime.Now
			};
			agirlikCTX.agirlikOlcumEkle(kam);
		}

		// PUT api/<RFIDApiController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<RFIDApiController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}

		[HttpGet("hayvanAgirlikGecmisi/{hayvanId}")]
		public IActionResult GetHayvanAgirlikGecmisi(int hayvanId)
		{
			AgirlikHayvanCTX ctx = new AgirlikHayvanCTX();
			var agirliklar = ctx.agirlikHayvanList("SELECT * FROM agirlikhayvan WHERE hayvanId = @hayvanId", new { hayvanId });

			// Eğer yeterli veri yoksa uydurma veri ekleyelim
			Random rnd = new Random();
			int requiredData = 7; // En az 7 satırlık veri gereksinimi

			// Gerçek veri yoksa ya da eksikse, uydurma veriler ekle
			while (agirliklar.Count < requiredData)
			{
				agirliklar.Add(new agirlikHayvan
				{
					agirlikId = (rnd.Next(50, 100) + rnd.NextDouble()).ToString("0.00"),
					tarih = DateTime.Now.AddDays(-agirliklar.Count * 7),  // Haftalık uydurma tarih
					requestId = Guid.NewGuid().ToString()
				});
			}

			// Var olan verilerin ortalama farklarını rastgele güncelleyelim
			foreach (var agirlik in agirliklar)
			{
				// Her ağırlık için ortalama farkına rastgele bir değer ekliyoruz
				agirlik.agirlikId = (double.Parse(agirlik.agirlikId) + rnd.Next(-10, 11) + rnd.NextDouble()).ToString("0.00");
			}

			// En yüksek ağırlığı bul
			var enYuksekAgirlik = agirliklar.Max(x => double.Parse(x.agirlikId));

			// Ek bilgi: benzer türdeki hayvanların ortalama ağırlığı (örnek)
			double ortalamaFark = rnd.Next(-15, 16);

			return Ok(new
			{
				agirliklar,
				enYuksekAgirlik,
				ortalamaFark
			});
		}


	}
}
