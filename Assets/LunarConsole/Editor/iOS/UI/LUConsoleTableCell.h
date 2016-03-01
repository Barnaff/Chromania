//
//  LUConsoleTableCell.h
//
//  Lunar Unity Mobile Console
//  https://github.com/SpaceMadness/lunar-unity-console
//
//  Copyright 2016 Alex Lementuev, SpaceMadness.
//
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
//
//      http://www.apache.org/licenses/LICENSE-2.0
//
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
//

#import <UIKit/UIKit.h>

@interface LUConsoleTableCell : UITableViewCell

@property (nonatomic, strong, nullable) UIImage  * icon;
@property (nonatomic, strong, nullable) NSString * message;
@property (nonatomic, strong, nullable) UIColor  * messageColor;
@property (nonatomic, strong, nullable) UIColor  * cellColor;

+ (nonnull instancetype)cellWithFrame:(CGRect)frame reuseIdentifier:(nullable NSString *)reuseIdentifier;
- (nonnull instancetype)initWithFrame:(CGRect)frame reuseIdentifier:(nullable NSString *)reuseIdentifier;

- (void)setSize:(CGSize)size;

+ (CGFloat)heightForCellWithText:(nullable NSString *)text width:(CGFloat)width;

@end

@interface LUConsoleTableCollapsedCell : LUConsoleTableCell

@property (nonatomic, assign) NSInteger collapsedCount;

@end