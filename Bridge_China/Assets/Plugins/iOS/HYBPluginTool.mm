#import "HYBPluginTool.h"
@implementation HYBPluginTool
void _savePhoto(char* readAddr)
{
    NSString* strreadaddr=[NSString stringWithUTF8String:readAddr];
    UIImage* img=[UIImage imageWithContentsOfFile:strreadaddr];
         
    UIImageWriteToSavedPhotosAlbum(img, nil, nil, nil);
}
@end