var div = document.createElement('div');
div.style.background="00000";
div.style.width="150px";
div.style.height="100px";
document.body.appendChild(div);
function createFlashMarkup(width,height,uri){

 var embed = document.createElement('embed');
 embed.setAttribute('width',width);
 embed.setAttribute('height',height);
 embed.setAttribute('src',uri);

 document.body.appendChild(embed); 
}
createFlashMarkup('550','400','http://vienna-add-in.googlecode.com/svn/wiki/videos/VIENNAaddInInstaller.swf');