INCLUDEPATH += \
        ../../../
HEADERS += \
        include/GeometryOperatorsContract.h \
        include/SpatialReference.h \
        include/SpatialReferenceEnums.h \
        include/Ellipsoid.h

SOURCES += \
        src/GeometryOperatorsContract.cpp \
        src/SpatialReference.cpp \
        src/Ellipsoid.cpp

OBJECTS_DIR = obj
DESTDIR = ../../../build
TARGET = GeometryOperatorsContract

TEMPLATE = lib
QT =
CONFIG += staticlib

win32 {
    DEFINES += WINDOWS
}

unix {
    DEFINES += LINUX
}
