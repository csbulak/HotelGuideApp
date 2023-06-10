using System.ComponentModel;

namespace Shared.Enums;

public enum ErrorCodes
{
    [Description("Beklenmeyen Bir Hata Meydana Geldi")]
    InternalServerError = 1000001,
    [Description("İlgili veri bulunamadı")]
    NotFound = 1000002,
    [Description("Oluşturma işlemi başarılı")]
    Created = 1000003,
    [Description("Güncelleme işlemi başarılı")]
    Updated = 1000004,
    [Description("Silme işlemi başarılı")]
    Deleted = 1000005,
    [Description("Gönderilen parametrelerde eksik veya yanlış bilgi mevcuttur")]
    BadRequest = 1000006,
    [Description("Validasyon hatası meydana geldi")]
    Validation = 1000007,
    [Description("Yetkiniz bulunmamaktadır")]
    UnAuthorized = 1000008,
    [Description("Hop! Giriş iznin yok!")]
    Forbidden = 1000009,
}
