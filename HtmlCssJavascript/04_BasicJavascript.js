function ucgenDoldur(karakter, satirSayisi, tersten = false) {
    var sonuc;
    var sonucArray = new Array();
    for (var i = 0; i < satirSayisi; i++) {
        sonuc = "";
        for (var j = 0; j <= i; j++) {
            sonuc += karakter;
        }
        sonucArray[i] = sonuc;
    }
    sonuc = "";
    if (tersten) {
        for (var i = sonucArray.length - 1; i >= 0; i--) {
            sonuc += sonucArray[i] + "\n";
        }
    } else {
        for (var i = 0; i < sonucArray.length; i++) {
            sonuc += sonucArray[i] + "\n";
        }
    }
    return sonuc;
}


function textAreaIlkDoldur(karakter, satirSayisi) {
    var textArea = document.getElementById("mytextareaid");
    var sonuc = ucgenDoldur(karakter, satirSayisi);
    textArea.value = sonuc;
}

var tersten = true;

function ucgeniTersineCevir(karakter, satirSayisi) {
    var textArea = document.getElementsByName("mytextareaname")[0];
    var sonuc = ucgenDoldur(karakter, satirSayisi, tersten);
    textArea.value = sonuc;
    var body = document.getElementsByTagName("body")[0];
    if (tersten) {
        body.style.backgroundColor = "blue";
    } else {
        body.style.backgroundColor = "yellow";
    }
    tersten = !tersten;
}
