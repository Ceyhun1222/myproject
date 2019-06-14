INCLUDEPATH += \
        ../../../

HEADERS += \
        include/EsriGeometry.h \
        include/EsriPacket.h \
        include/EsriGeometryCollection.h \
        include/ImportEsri.h \
        include/EsriMultiPoint.h \
        include/EsriPoint.h \
        include/EsriPointCollection.h \
        include/EsriPolyline.h \
        include/EsriPolygon.h \
        include/EsriRing.h \
        include/EsriPath.h

SOURCES += \
        src/EsriGeometry.cpp \
        src/EsriPacket.cpp \
        src/EsriGeometryCollection.cpp \
        src/EsriMultiPoint.cpp \
        src/EsriPoint.cpp \
        src/EsriPointCollection.cpp \
        src/EsriPolyline.cpp \
        src/EsriPolygon.cpp \
        src/EsriRing.cpp \
        src/EsriPath.cpp

OBJECTS_DIR = obj
DESTDIR = ../../../build

CONFIG += staticlib
QT =
TEMPLATE = lib

win32:DEFINES += WINDOWS
unix:DEFINES += LINUX
