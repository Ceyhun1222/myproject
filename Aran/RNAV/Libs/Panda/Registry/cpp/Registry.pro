QT = 
TEMPLATE = lib
CONFIG += dll release
TARGET = Registry
SOURCES += src/Object.cpp \
 src/ClassDirectory.cpp \
 src/LocalClass.cpp \
 src/Class.cpp \
 src/LocalPacket.cpp \
 src/Registry.cpp \
 src/Config.cpp \
 src/ObjectDirectory.cpp \
 src/BinaryPacket.cpp
HEADERS = include/ClassDirectory.h \
 include/Object.h \
 include/LocalClass.h \
 include/Class.h \
 include/LocalPacket.h \
 include/Packet.h \
 include/Contract.h \
 include/Config.h \
 include/ObjectDirectory.h \
 include/BinaryPacket.h
 
OBJECTS_DIR = obj
DESTDIR = ../../../build
DLLDESTDIR = ../../../../WorkFolder
unix {
 DEFINES +=  LINUX
}
win32 {
 DEFINES +=  WINDOWS
 DEF_FILE = Registry.def
}
