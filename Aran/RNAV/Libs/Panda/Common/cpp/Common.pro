INCLUDEPATH += \
        ../../../

HEADERS += \
        include/List.h \
        include/UnitConverter.h

SOURCES += \
        src/UnitConverter.cpp

OBJECTS_DIR = obj
DESTDIR = ../../../build
TARGET = Common

TEMPLATE = lib
QT =
CONFIG += staticlib

win32 {
    DEFINES += WINDOWS
}

unix {
    DEFINES += LINUX
}

