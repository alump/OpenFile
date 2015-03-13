/*
 * Copyright (C) 2015 Sami Viitanen <sami.viitanen@gmail.com>
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 * http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

#import "OpenFile.h"

@implementation OpenFile

- (void)open:(CDVInvokedUrlCommand *)command {

    CDVPluginResult *result = nil;
    NSString *path = [command.arguments firstObject];

    if (path && path.length > 0) {
        NSURL *url = [NSURL URLWithString:path];
        NSError *error;
        if (url.isFileURL && [url checkResourceIsReachableAndReturnError:&error] == YES) {
            self.url = url;
            QLPreviewController *previewController = [QLPreviewController new];
            previewController.delegate = self;
            previewController.dataSource = self;
            UIViewController *rvc = [[[UIApplication sharedApplication] keyWindow] rootViewController];
            [rvc presentViewController:previewController animated:YES completion:nil];
            NSLog(@"open successful");
            result = [CDVPluginResult resultWithStatus:CDVCommandStatus_OK];
        } else {
            NSLog(@"url is not valid");
            result = [CDVPluginResult resultWithStatus:CDVCommandStatus_ERROR messageAsString:@"File not found"];
        }
    } else {
        NSLog(@"no url or url is empty");
        result = [CDVPluginResult resultWithStatus:CDVCommandStatus_ERROR messageAsString:@"Invalid URI"];
    }

    [self.commandDelegate sendPluginResult:result callbackId:command.callbackId];
}

#pragma - QLPreviewItem Protocol

- (NSURL *)previewItemURL {
    return self.url;
}

#pragma - QLPreviewControllerDataSource Protocol

- (NSInteger)numberOfPreviewItemsInPreviewController:(QLPreviewController *)controller {
  return 1;
}

- (id<QLPreviewItem>)previewController:(QLPreviewController *)controller previewItemAtIndex:(NSInteger)index {
  return self;
}

@end