#import <Foundation/Foundation.h>
#import <UIKit/UIKit.h>
#import "HYBPluginTool.h"
@interface HYBPluginTool : NSObject
extern "C" {
    void _savePhoto(char* readAddr);
}
@end