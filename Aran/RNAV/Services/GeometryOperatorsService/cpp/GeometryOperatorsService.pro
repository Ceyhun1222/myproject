HEADERS +=
SOURCES += \
        src/GeometryOperatorsService.cpp

INCLUDEPATH += ../../../Libs

OBJECTS_DIR = obj
DLLDESTDIR = ../../../WorkFolder
TARGET = GeometryOperatorsService

TEMPLATE = lib
QT =
CONFIG += dll

win32-msvc2005 {
    DEFINES += WINDOWS
    LIBS += \
        ../../../Libs/build/Registry.lib \
        ../../../Libs/build/EsriClasses.lib \
        ../../../Libs/build/GeometryOperatorsContract.lib \
        ../../../Libs/build/Geometry.lib
    DEF_FILE = def/GeometryOperatorsService.def
}

win32-g++ {
    DEFINES += WINDOWS
    LIBS += \
        -L../../../Libs/build \
        -lEsriClasses \
        -lGeometry \
                -lGeometryOperatorContract \
        -lRegistry \
    DEF_FILE = def/GeometryOperatorsService.def
}

unix {
    DEFINES += LINUX
    LIBS += \
        -L../../../Libs/build \
        -lEsriClasses \
        -lGeometry \
                -lCommon \
        -lRegistry \
}
