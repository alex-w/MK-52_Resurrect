//////////////////////////////////////////////////////////
//
//  MK-52 RESURRECT
//  Copyright (c) 2020 Mike Yakimov.  All rights reserved.
//  See main file for the license
//
//////////////////////////////////////////////////////////

#include "Displays.hpp"

#define __DEBUG
using namespace MK52_Interpreter;

//
// Inits display for AUTO mode
//
unsigned long Display_AUTO::init( void *components[]) {
    //_nr = (MK52_Interpreter::Receiver_Number *)components[COMPONENT_RECEIVER_NUMBER];
    return Display::init( components);
}

int Display_AUTO::activate(){
    #ifdef __DEBUG
    long TargetTime = millis();
    #endif
    _lcd->dimScreen();
    _lcd->clearScreen( false);
    _lcd->outputStatus( _pmem->getCounter(), 5678, "DEG", "BBB");
    _lcd->outputCalcRegister( 0, _fakeData);
    _lcd->outputCalcLabel( 0, "X: should be a number");
    _lcd->outputCalcRegister( 1, NAN);
    _lcd->outputCalcLabel( 1, "Y: should be an Error");
    _lcd->outputCalcRegister( 2, +1.0/0.0);
    _lcd->outputCalcLabel( 2, "Z: should be +Inf");
    _lcd->outputCalcRegister( 3, -1.0/0.0);
    _lcd->outputCalcLabel( 3, "T: should be -Inf");
    _lcd->undimScreen();
    #ifdef __DEBUG
    TargetTime = millis() - TargetTime;
    Serial.print ("AUTO display activated in ");
    Serial.print ( TargetTime);
    Serial.println (" ms");
    delay( DEBUG_SHOW_DELAY);
    #endif
    return -1;
}

void Display_AUTO::tick(){
    if( _nr->isActive()){
        _lcd->updateStatus( _pmem->getCounter(), 5678, "DEG", "NUM");
        _lcd->updateCalcRegister( 0, _nr->toString());
    }
    else{
        _lcd->updateStatus( _pmem->getCounter(), 5678, "DEG", "   ");
        _lcd->updateCalcRegister( 0, _fakeData);
    }    
}
