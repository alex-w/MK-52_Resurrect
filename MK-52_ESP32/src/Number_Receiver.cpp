//////////////////////////////////////////////////////////
//
//  MK-52 RESURRECT
//  Copyright (c) 2020 Mike Yakimov.  All rights reserved.
//  See main file for the license
//
//////////////////////////////////////////////////////////

#include "Input_Receivers.hpp"

const char _NR_ButtonConversion[] PROGMEM = "#############7410852.963-###E##ec";
const char _NR_StandardUnity[] PROGMEM = "1.0E+000";

using namespace MK52_Interpreter;

//
// Inits number entry
//
unsigned long Number_Receiver::init( void *components[]) {
    return Input_Receiver::init(components);
}

void Number_Receiver::activate( int parentReturn, uint8_t scancode){
    Input_Receiver::activate( parentReturn, scancode);
    strcpy_P( _text, PSTR(" "));
    _mode = 1;
    if(!scancode) return;
    _appendChar( _convertButton( scancode));
}

int Number_Receiver::tick( uint8_t scancode){
    if(scancode == 0) scancode = _kbd->scan();
    if( !scancode) return NO_PARENT_RETURN;
    return _appendChar( _convertButton( scancode));
}

char Number_Receiver::_convertButton(uint8_t scancode){
    char c = (char)pgm_read_byte(_NR_ButtonConversion + scancode);
    if( c == '#') return 0;
    return c;
}

int Number_Receiver::_appendChar( char c){
    int ln = strlen(_text);
    switch( c){
      case 0:
          return NO_PARENT_RETURN;
      case '0':
      case '1':
      case '2':
      case '3':
      case '4':
      case '5':
      case '6':
      case '7':
      case '8':
      case '9':
          if( ln == 2 && _text[1] == '0'){
              _text[1] = c;
              return NO_PARENT_RETURN;
          }
          if( _mode == 1 && ln>12) return NO_PARENT_RETURN;
          if( _mode == 2 && ln>13) return NO_PARENT_RETURN;
          if( _mode == 3){
              _text[ln-3] = _text[ln-2]; 
              _text[ln-2] = _text[ln-1]; 
              _text[ln-1] = c; 
              return NO_PARENT_RETURN;
          }
          _text[ln] = c;
          _text[ln+1] = 0;
          return NO_PARENT_RETURN;
      case '-':
          if( _mode == 3){
              _swapSign( _text + ln - 4, '+');
              return NO_PARENT_RETURN;
          }
          _swapSign( _text, ' ');
          return NO_PARENT_RETURN;
      case '.':
          if( _mode >= 2) return NO_PARENT_RETURN;
          _mode = 2;
          if( ln == 1){
              strcpy_P( _text+1, PSTR("0."));
              return NO_PARENT_RETURN;
          }
          strcpy_P( _text+ln, PSTR("."));
          return NO_PARENT_RETURN;
      case 'E':
          if( _mode == 3){
              _text[ln-5] = 0;
              _mode = 2;
              return NO_PARENT_RETURN;
          }
          if( ln == 1 || strcmp_P(_text+1, PSTR("0.0")) == 0 || strcmp_P(_text+1, PSTR("0.")) == 0){
              strcpy_P( _text+1, _NR_StandardUnity);
              _mode = 3;
              return NO_PARENT_RETURN;
          }
          if( ln == 2 && _text[1] == '0'){
              strcpy_P( _text+1, _NR_StandardUnity);
              _mode = 3;
              return NO_PARENT_RETURN;
          }
          if( _mode == 1){
              strcpy_P( _text+ln, _NR_StandardUnity+1);
              _mode = 3;
              return NO_PARENT_RETURN;
          }
          _mode = 3;
          if( _text[ln-1] == '.'){
              strcpy_P( _text+ln, _NR_StandardUnity+2);
              return NO_PARENT_RETURN;
          }
          strcpy_P( _text+ln, _NR_StandardUnity+3);
          return NO_PARENT_RETURN;
      case 'e': // entry completed
          _mode = 0;
          return _parentReturn;
      case 'c': // erase
          if( _mode == 3){
              _text[ln-1] = _text[ln-2];
              _text[ln-2] = _text[ln-3];
              _text[ln-3] = '0';
              return NO_PARENT_RETURN;
          }
          if( _mode == 2 && _text[ln-1] == '.'){
              _text[ln-1] = 0;
              _mode = 1;
              return NO_PARENT_RETURN;
          }
          if( ln>1){
              _text[ln-1] = 0;
              return NO_PARENT_RETURN;
          }
          strcpy_P( _text, PSTR(" 0"));
          _mode = 0;
          return _parentReturn;
      default:
          return NO_PARENT_RETURN;
    }
}
