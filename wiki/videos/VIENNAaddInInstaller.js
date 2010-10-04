function createFlashMarkup(width,height,uri,replaceid){

 var embed = document.createElement('embed');
 embed.setAttribute('width',width);
 embed.setAttribute('height',height);
 embed.setAttribute('src',uri);

 var div = document.getElementById(replaceid);
 document.getElementsByTagName('body')[0].replaceChild(embed,div); 
}
window.onload = function(){
createFlashMarkup('550','400','VIENNAaddInInstaller.swf','replaced-by-flash');
}