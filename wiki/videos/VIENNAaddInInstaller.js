function createFlashMarkup(width,height,uri){

 var embed = document.createElement('embed');
 embed.setAttribute('width',width);
 embed.setAttribute('height',height);
 embed.setAttribute('src',uri);

 document.body.appendChild(embed); 
}
window.onload = function(){
createFlashMarkup('550','400','http://vienna-add-in.googlecode.com/svn/wiki/videos/VIENNAaddInInstaller.swf');
}