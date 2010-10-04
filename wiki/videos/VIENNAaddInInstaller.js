function createFlashMarkup(width,height,uri){

 var embed = document.createElement('embed');
 embed.setAttribute('width',width);
 embed.setAttribute('height',height);
 embed.setAttribute('src',uri);

 document.body.appendChild(embed); 
}
createFlashMarkup('1024','768','http://vienna-add-in.googlecode.com/svn/wiki/videos/VIENNAaddInInstaller.swf');