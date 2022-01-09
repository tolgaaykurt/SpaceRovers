# SPACE ROVERs #
**PROJE AÇIKLAMASI**

Projenin amacı rover'ları platoya indirmek, yönlerini değiştirmek ve platoda kazasız ilerletmek.

**NASIL KULLANILIR?**

Projeyi build etmek yeterlidir. Açılan form'da gözüken test butonları kullanılarak görsel testler gerçekleştirilebilir. Test projesi ile unit test'ler kontrol edilebilir.

**TEKNİK BİLGİLER**

Yön değişimlerini takip edebilmek için *State Pattern* kullanıldı. Rover'ların hareket durumlarını takip edebilmek için *Observer Pattern* kullanıldı. Text komutların işlenebilmesi için *Chain Of Resposibility Pattern* kullanıldı. 

Rover'ların hareketlerinin takip edilebilmesi için çıktılar Visual Studio output penceresine yazdırıldı. Rover hareketleri aynı zamanda ekranda görsel olarakta simüle edildi.

![SpaceRover_Project_Demo](https://user-images.githubusercontent.com/10353442/148481267-d7bd6e08-eb1a-49cb-89a3-f8f6cb974b17.gif)
