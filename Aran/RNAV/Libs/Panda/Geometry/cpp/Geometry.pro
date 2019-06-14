INCLUDEPATH += \
        ../../../

HEADERS += \
        include/NullGeometry.h \
        include/GeometryType.h \
        include/Geometry.h \
        include/GeometryPacket.h \
        include/Point.h \
        include/MultiPoint.h \
        include/Part.h \
        include/Ring.h\
        include/Poly.h \
        include/Polyline.h \
        include/Polygon.h

SOURCES += \
        src/Geometry.cpp \
        src/GeometryPacket.cpp \
        src/Point.cpp \
        src/MultiPoint.cpp \
        src/Part.cpp \
        src/Ring.cpp \
        src/Polyline.cpp \
        src/Polygon.cpp

OBJECTS_DIR = obj
DESTDIR = ../../../build

TEMPLATE = lib
QT =
CONFIG += staticlib

win32:DEFINES += WINDOWS
unix:DEFINES += LINUX

