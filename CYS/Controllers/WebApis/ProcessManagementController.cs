using CYS.Models;
using CYS.Repos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Plugins;

namespace CYS.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProcessManagementController : ControllerBase
    {
        private readonly processmanagementCTX _context;

        public ProcessManagementController()
        {
            _context = new processmanagementCTX();
        }

        [HttpGet("{cihazId}")]
        public IActionResult GetProcessManagementByCihazId(int cihazId)
        {
            // Sorguyu cihazId ve isTamamlandi = 0 olan kayıtları kontrol etmek için yazıyoruz
            string sorgu = "SELECT id, mevcutMod, minimumhassasiyetagirlik, cikisbeklemeagirligi FROM processmanagement WHERE cihazId = @cihazId AND isTamamlandi = 0";
            var param = new { cihazId };

            var result = _context.Get(sorgu, param);

            if (result != null)
            {
                // Eğer kayıt varsa, id, mevcut mod ve hassasiyet ağırlıklarını döndürüyoruz
                return Ok(new
                {
                    id = result.id,
                    mevcutMod = result.mevcutmod,
                    girisHassasiyetAgirligi = result.minimumhassasiyetagirlik,
                    cikisHassasiyetAgirligi = result.cikisbeklemeagirligi
                });
            }
            else
            {
                // Eğer kayıt yoksa yeni bir kayıt oluşturulacak
                var newProcess = new processmanagement
                {
                    guid = Guid.NewGuid().ToString(),
                    cihazId = cihazId,
                    hayvangirdi = 0,
                    ilkkapikapandi = 0,
                    kupeokundu = 0,
                    okunankupe = "",
                    sonagirlikalindimi = 0,
                    sonagirlik = 0,
                    cikiskapisiacildimi = 0,
                    tarih = DateTime.Now,
                    cikisbeklemeagirligi = 5.0f,  // Varsayılan bir çıkış hassasiyet ağırlığı
                    minimumhassasiyetagirlik = 5.0f,  // Varsayılan bir giriş hassasiyet ağırlığı
                    isTamamlandi = 0,
                    mevcutmod = 1, // Varsayılan bir mod değeri veriyoruz
                    tamamlanmatarihi = DateTime.MinValue
                };

                var gelen = _context.Add(newProcess);

                // Yeni oluşturulan kaydın id, mevcut mod ve hassasiyet ağırlıklarını döndürüyoruz
                return Ok(new
                {
                    id = gelen,
                    mevcutMod = newProcess.mevcutmod,
                    girisHassasiyetAgirligi = newProcess.minimumhassasiyetagirlik,
                    cikisHassasiyetAgirligi = newProcess.cikisbeklemeagirligi
                });
            }
        }
        public class AgirlikRequestModel
        {
            public float agirlik { get; set; }
            public int cihazId { get; set; }
        }
        [HttpPost]
        public IActionResult Post([FromBody] AgirlikRequestModel model)
        {
            // İlgili cihazId'deki aktif (isTamamlandi = 0) olan process'i kontrol ediyoruz
            string sorgu = "SELECT * FROM processmanagement WHERE cihazId = @cihazId AND isTamamlandi = 0";
            var param = new { model.cihazId };

            // Aktif process'i alıyoruz
            var result = _context.Get(sorgu, param);

            if (result != null && result.mevcutmod == 4)
            {
                // Eğer mevcut mod 6 ise ağırlık ölçümünü kaydediyoruz
                AgirlikOlcumCTX agirlikCTX = new AgirlikOlcumCTX();
                AgirlikOlcum yeniAgirlik = new AgirlikOlcum()
                {
                    agirlikOlcumu = model.agirlik.ToString(),  // Ağırlık değeri string olarak kaydediliyor
                    userId = 1,  // Varsayılan kullanıcı ID'si, bu ihtiyaçlarınıza göre güncellenebilir
                    tarih = DateTime.Now,  // Kayıt zamanı
                    requestId = result.guid  // processmanagement'taki guid, requestId olarak kullanılıyor
                };

                // Ağırlık ölçümünü kaydediyoruz
                agirlikCTX.agirlikOlcumEkle(yeniAgirlik);
                result.sonagirlik = model.agirlik;
                result.sonagirlikalindimi = 1;
                result.mevcutmod = 7;
                _context.Update(result);

                return Ok(new
                {
                    id = result.id,
                    mevcutMod = result.mevcutmod,
                    girisHassasiyetAgirligi = result.minimumhassasiyetagirlik,
                    cikisHassasiyetAgirligi = result.cikisbeklemeagirligi
                });
            }
            else
            {
                // Eğer mevcut mod 6 değilse ya da process bulunamazsa hata döndürülür
                return BadRequest(new { message = "Ağırlık ölçümü yalnızca mevcut mod 6 iken yapılabilir." });
            }
        }

        [HttpPatch("{cihazId}")]
        public IActionResult UpdateHayvanGirdiByCihazId(int cihazId, [FromBody] int yeniHayvanGirdi)
        {
            // İlgili cihazId'deki processmanagement kaydını buluyoruz
            string sorgu = "SELECT * FROM processmanagement WHERE cihazId = @cihazId AND isTamamlandi = 0";
            var param = new { cihazId };

            var result = _context.Get(sorgu, param);

            if (result != null)
            {
                // Hayvangirdi değerini güncelliyoruz
                result.hayvangirdi = yeniHayvanGirdi;

                // Processmanagement tablosundaki kaydı güncelliyoruz
                _context.Update(result);

                // Güncellenen kaydın bilgilerini döndürüyoruz
                return Ok(new
                {
                    id = result.id,
                    cihazId = result.cihazId,
                    hayvangirdi = result.hayvangirdi,
                    message = "Hayvan girdi başarıyla güncellendi."
                });
            }
            else
            {
                // Eğer kayıt bulunamazsa hata döndürüyoruz
                return NotFound(new { message = "İlgili cihazId'ye ait aktif bir process kaydı bulunamadı." });
            }
        }
        [HttpPatch("ilkkapikapandi/{cihazId}")]
        public IActionResult UpdateIlkKapiKapandiByCihazId(int cihazId, [FromBody] int ilkkapikapandi)
        {
            // İlgili cihazId'deki processmanagement kaydını buluyoruz
            string sorgu = "SELECT * FROM processmanagement WHERE cihazId = @cihazId AND isTamamlandi = 0";
            var param = new { cihazId };

            var result = _context.Get(sorgu, param);

            if (result != null)
            {
                // ilkkapikapandi değerini güncelliyoruz
                result.ilkkapikapandi = ilkkapikapandi;

                // Processmanagement tablosundaki kaydı güncelliyoruz
                _context.Update(result);

                // Güncellenen kaydın bilgilerini döndürüyoruz
                return Ok(new
                {
                    id = result.id,
                    cihazId = result.cihazId,
                    ilkkapikapandi = result.ilkkapikapandi,
                    message = "İlk kapı kapandı değeri başarıyla güncellendi."
                });
            }
            else
            {
                // Eğer kayıt bulunamazsa hata döndürüyoruz
                return NotFound(new { message = "İlgili cihazId'ye ait aktif bir process kaydı bulunamadı." });
            }
        }

        [HttpPost]
        public IActionResult KupeAtamaGuncelle([FromBody] string kupeRfid, int cihazId)
        {
            string processSorgu = "SELECT * FROM processmanagement WHERE cihazId = @cihazId AND isTamamlandi = 0";
            var processParam = new { cihazId };

            var processContext = new processmanagementCTX();
            var process = processContext.Get(processSorgu, processParam);
            if(process.mevcutmod != 3)
            {
                return Ok(new
                {
                    message = "mod yanlis"
                });
            }

            // 1. Küpe atanmış mı kontrol ediyoruz
            string sorgu = "SELECT * FROM kupehayvan WHERE kupeId = @kupeRfid";
            var param = new { kupeRfid };

            kupehayvanCTX kupeHayvanContext = new kupehayvanCTX();
            var kupeHayvan = kupeHayvanContext.kupehayvanTek(sorgu, param);

            if (kupeHayvan != null)
            {
                // 2. Küpe atanmışsa, cihazId'ye bağlı processmanagement'taki hayvanId'yi güncelle
               

                if (process != null)
                {
                    process.hayvanid = kupeHayvan.hayvanId;
                    process.okunankupe = kupeRfid;  // Okunan küpeyi güncelliyoruz
                    process.kupeokundu = 1;  // Küpe okundu
                    processContext.Update(process);
                }

                return Ok(new
                {
                    message = "Küpe zaten atanmıştı, hayvanId process'te güncellendi.",
                    hayvanId = kupeHayvan.hayvanId,
                    cihazId = cihazId
                });
            }
            else
            {
                // 3. Küpe atanmadıysa yeni hayvan ekle
                HayvanCTX hayvanContext = new HayvanCTX();
                Hayvan yeniHayvan = new Hayvan()
                {
                    rfidKodu = kupeRfid,
                    kupeIsmi = kupeRfid,  // Küpe ismi küpe RFID ile aynı olabilir
                    cinsiyet = "Bilinmiyor",  // Varsayılan olarak bilinmiyor olabilir
                    agirlik = "Bilinmiyor",  // Varsayılan olarak bilinmiyor olabilir
                    userId = 1,  // Kullanıcı ID'si varsayılan olarak belirlenebilir
                    kategoriId = 1,  // Varsayılan kategori
                    requestId = Guid.NewGuid().ToString(),  // Unique requestId
                    tarih = DateTime.Now,
                    aktif = 1
                };

                // Yeni hayvanı ekliyoruz
                hayvanContext.hayvanEkle(yeniHayvan);

                // Yeni hayvanı kupehayvan tablosuna ekliyoruz
                kupehayvan yeniKupeHayvan = new kupehayvan()
                {
                    hayvanId = yeniHayvan.id,
                    kupeId = kupeRfid,
                    requestId = yeniHayvan.requestId,
                    tarih = DateTime.Now
                };

                kupeHayvanContext.kupehayvanEkle(yeniKupeHayvan);

                // 4. processmanagement'ta hayvan ve okunankupe alanlarını güncelle
                

                if (process != null)
                {
                    process.hayvanid = yeniHayvan.id;
                    process.okunankupe = kupeRfid;  // Okunan küpeyi güncelliyoruz
                    process.kupeokundu = 1;  // Küpe okundu
                    processContext.Update(process);
                }

                return Ok(new
                {
                    message = "Yeni hayvan oluşturuldu ve küpe bilgisi güncellendi.",
                    hayvanId = yeniHayvan.id,
                    cihazId = cihazId
                });
            }
        }



    }
}
