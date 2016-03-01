//
//  LUDefines.h
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

#define LU_SHOULD_IMPLEMENT_METHOD \
    NSLog(@"%@ should implement %@ method", NSStringFromClass([self class]), NSStringFromSelector(_cmd));

#if __has_feature(objc_arc)
    #define LU_WEAK __weak
    #define LU_RETAIN(obj) (obj)
    #define LU_RELEASE(obj)
    #define LU_AUTORELEASE(obj) (obj)
    #define LU_SUPER_DEALLOC
#else
    #define LU_WEAK
    #define LU_RETAIN(obj) [(obj) retain]
    #define LU_RELEASE(obj) [(obj) release]
    #define LU_AUTORELEASE(obj) [(obj) autorelease]
    #define LU_SUPER_DEALLOC [super dealloc];
#endif