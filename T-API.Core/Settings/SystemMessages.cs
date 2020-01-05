namespace T_API.Core.Settings
{
    public class SystemMessages
    {
        public static string UnauthorizedOperationExceptionMessage => "İşlem yapılmak istenilen veri erişimi kısıtlandırılmıştır.";
        public static string InvalidOperationExceptionMessage => "Yapılmak istenilen işlem geçerli değildir.Lütfen sistem yöneticiniz ile iletişime geçiniz.";
        public static string DuringOperationExceptionMessage=> "İşlem yapılırken veri tabanında hata oluştu lütfen daha sonra tekrar deneyiniz.";
        public static string NoContentExceptionMessage => "İşlem yapılırken veri tabanında hata oluştu lütfen daha sonra tekrar deneyiniz.";
        public static string SuccessMessage => "İşlem başarıyla gerçekleştirildi.";
        public static string LoginSuccessMessage => "Başarıyla Giriş Yapıldı.Hoş Geldiniz..";
        
    }
    
}