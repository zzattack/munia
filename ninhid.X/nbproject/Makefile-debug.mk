#
# Generated Makefile - do not edit!
#
# Edit the Makefile in the project folder instead (../Makefile). Each target
# has a -pre and a -post target defined where you can add customized code.
#
# This makefile implements configuration specific macros and targets.


# Include project Makefile
ifeq "${IGNORE_LOCAL}" "TRUE"
# do not include local makefile. User is passing all local related variables already
else
include Makefile
# Include makefile containing local settings
ifeq "$(wildcard nbproject/Makefile-local-debug.mk)" "nbproject/Makefile-local-debug.mk"
include nbproject/Makefile-local-debug.mk
endif
endif

# Environment
MKDIR=gnumkdir -p
RM=rm -f 
MV=mv 
CP=cp 

# Macros
CND_CONF=debug
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
IMAGE_TYPE=debug
OUTPUT_SUFFIX=elf
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=dist/${CND_CONF}/${IMAGE_TYPE}/ninhid.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
else
IMAGE_TYPE=production
OUTPUT_SUFFIX=hex
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=dist/${CND_CONF}/${IMAGE_TYPE}/ninhid.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
endif

# Object Directory
OBJECTDIR=build/${CND_CONF}/${IMAGE_TYPE}

# Distribution Directory
DISTDIR=dist/${CND_CONF}/${IMAGE_TYPE}

# Source Files Quoted if spaced
SOURCEFILES_QUOTED_IF_SPACED=main.c usb_descriptors.c usb/src/usb_device.c usb/src/usb_device_hid.c xlcd/busyxlcd.c xlcd/openxlcd.c xlcd/putrxlcd.c xlcd/putsxlcd.c xlcd/readaddr.c xlcd/readdata.c xlcd/setcgram.c xlcd/setddram.c xlcd/wcmdxlcd.c xlcd/writdata.c

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/main.p1 ${OBJECTDIR}/usb_descriptors.p1 ${OBJECTDIR}/usb/src/usb_device.p1 ${OBJECTDIR}/usb/src/usb_device_hid.p1 ${OBJECTDIR}/xlcd/busyxlcd.p1 ${OBJECTDIR}/xlcd/openxlcd.p1 ${OBJECTDIR}/xlcd/putrxlcd.p1 ${OBJECTDIR}/xlcd/putsxlcd.p1 ${OBJECTDIR}/xlcd/readaddr.p1 ${OBJECTDIR}/xlcd/readdata.p1 ${OBJECTDIR}/xlcd/setcgram.p1 ${OBJECTDIR}/xlcd/setddram.p1 ${OBJECTDIR}/xlcd/wcmdxlcd.p1 ${OBJECTDIR}/xlcd/writdata.p1
POSSIBLE_DEPFILES=${OBJECTDIR}/main.p1.d ${OBJECTDIR}/usb_descriptors.p1.d ${OBJECTDIR}/usb/src/usb_device.p1.d ${OBJECTDIR}/usb/src/usb_device_hid.p1.d ${OBJECTDIR}/xlcd/busyxlcd.p1.d ${OBJECTDIR}/xlcd/openxlcd.p1.d ${OBJECTDIR}/xlcd/putrxlcd.p1.d ${OBJECTDIR}/xlcd/putsxlcd.p1.d ${OBJECTDIR}/xlcd/readaddr.p1.d ${OBJECTDIR}/xlcd/readdata.p1.d ${OBJECTDIR}/xlcd/setcgram.p1.d ${OBJECTDIR}/xlcd/setddram.p1.d ${OBJECTDIR}/xlcd/wcmdxlcd.p1.d ${OBJECTDIR}/xlcd/writdata.p1.d

# Object Files
OBJECTFILES=${OBJECTDIR}/main.p1 ${OBJECTDIR}/usb_descriptors.p1 ${OBJECTDIR}/usb/src/usb_device.p1 ${OBJECTDIR}/usb/src/usb_device_hid.p1 ${OBJECTDIR}/xlcd/busyxlcd.p1 ${OBJECTDIR}/xlcd/openxlcd.p1 ${OBJECTDIR}/xlcd/putrxlcd.p1 ${OBJECTDIR}/xlcd/putsxlcd.p1 ${OBJECTDIR}/xlcd/readaddr.p1 ${OBJECTDIR}/xlcd/readdata.p1 ${OBJECTDIR}/xlcd/setcgram.p1 ${OBJECTDIR}/xlcd/setddram.p1 ${OBJECTDIR}/xlcd/wcmdxlcd.p1 ${OBJECTDIR}/xlcd/writdata.p1

# Source Files
SOURCEFILES=main.c usb_descriptors.c usb/src/usb_device.c usb/src/usb_device_hid.c xlcd/busyxlcd.c xlcd/openxlcd.c xlcd/putrxlcd.c xlcd/putsxlcd.c xlcd/readaddr.c xlcd/readdata.c xlcd/setcgram.c xlcd/setddram.c xlcd/wcmdxlcd.c xlcd/writdata.c


CFLAGS=
ASFLAGS=
LDLIBSOPTIONS=

############# Tool locations ##########################################
# If you copy a project from one host to another, the path where the  #
# compiler is installed may be different.                             #
# If you open this project with MPLAB X in the new host, this         #
# makefile will be regenerated and the paths will be corrected.       #
#######################################################################
# fixDeps replaces a bunch of sed/cat/printf statements that slow down the build
FIXDEPS=fixDeps

.build-conf:  ${BUILD_SUBPROJECTS}
ifneq ($(INFORMATION_MESSAGE), )
	@echo $(INFORMATION_MESSAGE)
endif
	${MAKE}  -f nbproject/Makefile-debug.mk dist/${CND_CONF}/${IMAGE_TYPE}/ninhid.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

MP_PROCESSOR_OPTION=18LF24K50
# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/main.p1: main.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/main.p1.d 
	@${RM} ${OBJECTDIR}/main.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/main.p1  main.c 
	@-${MV} ${OBJECTDIR}/main.d ${OBJECTDIR}/main.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/main.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb_descriptors.p1: usb_descriptors.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/usb_descriptors.p1.d 
	@${RM} ${OBJECTDIR}/usb_descriptors.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb_descriptors.p1  usb_descriptors.c 
	@-${MV} ${OBJECTDIR}/usb_descriptors.d ${OBJECTDIR}/usb_descriptors.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb_descriptors.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb/src/usb_device.p1: usb/src/usb_device.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/usb/src" 
	@${RM} ${OBJECTDIR}/usb/src/usb_device.p1.d 
	@${RM} ${OBJECTDIR}/usb/src/usb_device.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb/src/usb_device.p1  usb/src/usb_device.c 
	@-${MV} ${OBJECTDIR}/usb/src/usb_device.d ${OBJECTDIR}/usb/src/usb_device.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb/src/usb_device.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb/src/usb_device_hid.p1: usb/src/usb_device_hid.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/usb/src" 
	@${RM} ${OBJECTDIR}/usb/src/usb_device_hid.p1.d 
	@${RM} ${OBJECTDIR}/usb/src/usb_device_hid.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb/src/usb_device_hid.p1  usb/src/usb_device_hid.c 
	@-${MV} ${OBJECTDIR}/usb/src/usb_device_hid.d ${OBJECTDIR}/usb/src/usb_device_hid.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb/src/usb_device_hid.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/busyxlcd.p1: xlcd/busyxlcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/busyxlcd.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/busyxlcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/busyxlcd.p1  xlcd/busyxlcd.c 
	@-${MV} ${OBJECTDIR}/xlcd/busyxlcd.d ${OBJECTDIR}/xlcd/busyxlcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/busyxlcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/openxlcd.p1: xlcd/openxlcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/openxlcd.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/openxlcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/openxlcd.p1  xlcd/openxlcd.c 
	@-${MV} ${OBJECTDIR}/xlcd/openxlcd.d ${OBJECTDIR}/xlcd/openxlcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/openxlcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/putrxlcd.p1: xlcd/putrxlcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/putrxlcd.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/putrxlcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/putrxlcd.p1  xlcd/putrxlcd.c 
	@-${MV} ${OBJECTDIR}/xlcd/putrxlcd.d ${OBJECTDIR}/xlcd/putrxlcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/putrxlcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/putsxlcd.p1: xlcd/putsxlcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/putsxlcd.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/putsxlcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/putsxlcd.p1  xlcd/putsxlcd.c 
	@-${MV} ${OBJECTDIR}/xlcd/putsxlcd.d ${OBJECTDIR}/xlcd/putsxlcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/putsxlcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/readaddr.p1: xlcd/readaddr.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/readaddr.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/readaddr.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/readaddr.p1  xlcd/readaddr.c 
	@-${MV} ${OBJECTDIR}/xlcd/readaddr.d ${OBJECTDIR}/xlcd/readaddr.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/readaddr.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/readdata.p1: xlcd/readdata.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/readdata.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/readdata.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/readdata.p1  xlcd/readdata.c 
	@-${MV} ${OBJECTDIR}/xlcd/readdata.d ${OBJECTDIR}/xlcd/readdata.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/readdata.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/setcgram.p1: xlcd/setcgram.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/setcgram.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/setcgram.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/setcgram.p1  xlcd/setcgram.c 
	@-${MV} ${OBJECTDIR}/xlcd/setcgram.d ${OBJECTDIR}/xlcd/setcgram.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/setcgram.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/setddram.p1: xlcd/setddram.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/setddram.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/setddram.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/setddram.p1  xlcd/setddram.c 
	@-${MV} ${OBJECTDIR}/xlcd/setddram.d ${OBJECTDIR}/xlcd/setddram.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/setddram.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/wcmdxlcd.p1: xlcd/wcmdxlcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/wcmdxlcd.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/wcmdxlcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/wcmdxlcd.p1  xlcd/wcmdxlcd.c 
	@-${MV} ${OBJECTDIR}/xlcd/wcmdxlcd.d ${OBJECTDIR}/xlcd/wcmdxlcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/wcmdxlcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/writdata.p1: xlcd/writdata.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/writdata.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/writdata.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/writdata.p1  xlcd/writdata.c 
	@-${MV} ${OBJECTDIR}/xlcd/writdata.d ${OBJECTDIR}/xlcd/writdata.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/writdata.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
else
${OBJECTDIR}/main.p1: main.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/main.p1.d 
	@${RM} ${OBJECTDIR}/main.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/main.p1  main.c 
	@-${MV} ${OBJECTDIR}/main.d ${OBJECTDIR}/main.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/main.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb_descriptors.p1: usb_descriptors.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/usb_descriptors.p1.d 
	@${RM} ${OBJECTDIR}/usb_descriptors.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb_descriptors.p1  usb_descriptors.c 
	@-${MV} ${OBJECTDIR}/usb_descriptors.d ${OBJECTDIR}/usb_descriptors.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb_descriptors.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb/src/usb_device.p1: usb/src/usb_device.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/usb/src" 
	@${RM} ${OBJECTDIR}/usb/src/usb_device.p1.d 
	@${RM} ${OBJECTDIR}/usb/src/usb_device.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb/src/usb_device.p1  usb/src/usb_device.c 
	@-${MV} ${OBJECTDIR}/usb/src/usb_device.d ${OBJECTDIR}/usb/src/usb_device.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb/src/usb_device.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb/src/usb_device_hid.p1: usb/src/usb_device_hid.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/usb/src" 
	@${RM} ${OBJECTDIR}/usb/src/usb_device_hid.p1.d 
	@${RM} ${OBJECTDIR}/usb/src/usb_device_hid.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb/src/usb_device_hid.p1  usb/src/usb_device_hid.c 
	@-${MV} ${OBJECTDIR}/usb/src/usb_device_hid.d ${OBJECTDIR}/usb/src/usb_device_hid.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb/src/usb_device_hid.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/busyxlcd.p1: xlcd/busyxlcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/busyxlcd.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/busyxlcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/busyxlcd.p1  xlcd/busyxlcd.c 
	@-${MV} ${OBJECTDIR}/xlcd/busyxlcd.d ${OBJECTDIR}/xlcd/busyxlcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/busyxlcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/openxlcd.p1: xlcd/openxlcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/openxlcd.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/openxlcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/openxlcd.p1  xlcd/openxlcd.c 
	@-${MV} ${OBJECTDIR}/xlcd/openxlcd.d ${OBJECTDIR}/xlcd/openxlcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/openxlcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/putrxlcd.p1: xlcd/putrxlcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/putrxlcd.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/putrxlcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/putrxlcd.p1  xlcd/putrxlcd.c 
	@-${MV} ${OBJECTDIR}/xlcd/putrxlcd.d ${OBJECTDIR}/xlcd/putrxlcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/putrxlcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/putsxlcd.p1: xlcd/putsxlcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/putsxlcd.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/putsxlcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/putsxlcd.p1  xlcd/putsxlcd.c 
	@-${MV} ${OBJECTDIR}/xlcd/putsxlcd.d ${OBJECTDIR}/xlcd/putsxlcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/putsxlcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/readaddr.p1: xlcd/readaddr.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/readaddr.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/readaddr.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/readaddr.p1  xlcd/readaddr.c 
	@-${MV} ${OBJECTDIR}/xlcd/readaddr.d ${OBJECTDIR}/xlcd/readaddr.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/readaddr.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/readdata.p1: xlcd/readdata.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/readdata.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/readdata.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/readdata.p1  xlcd/readdata.c 
	@-${MV} ${OBJECTDIR}/xlcd/readdata.d ${OBJECTDIR}/xlcd/readdata.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/readdata.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/setcgram.p1: xlcd/setcgram.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/setcgram.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/setcgram.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/setcgram.p1  xlcd/setcgram.c 
	@-${MV} ${OBJECTDIR}/xlcd/setcgram.d ${OBJECTDIR}/xlcd/setcgram.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/setcgram.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/setddram.p1: xlcd/setddram.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/setddram.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/setddram.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/setddram.p1  xlcd/setddram.c 
	@-${MV} ${OBJECTDIR}/xlcd/setddram.d ${OBJECTDIR}/xlcd/setddram.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/setddram.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/wcmdxlcd.p1: xlcd/wcmdxlcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/wcmdxlcd.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/wcmdxlcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/wcmdxlcd.p1  xlcd/wcmdxlcd.c 
	@-${MV} ${OBJECTDIR}/xlcd/wcmdxlcd.d ${OBJECTDIR}/xlcd/wcmdxlcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/wcmdxlcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/xlcd/writdata.p1: xlcd/writdata.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/xlcd" 
	@${RM} ${OBJECTDIR}/xlcd/writdata.p1.d 
	@${RM} ${OBJECTDIR}/xlcd/writdata.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/xlcd/writdata.p1  xlcd/writdata.c 
	@-${MV} ${OBJECTDIR}/xlcd/writdata.d ${OBJECTDIR}/xlcd/writdata.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/xlcd/writdata.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assemble
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
dist/${CND_CONF}/${IMAGE_TYPE}/ninhid.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk    
	@${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_CC} $(MP_EXTRA_LD_PRE) --chip=$(MP_PROCESSOR_OPTION) -G -mdist/${CND_CONF}/${IMAGE_TYPE}/ninhid.X.${IMAGE_TYPE}.map  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"        -odist/${CND_CONF}/${IMAGE_TYPE}/ninhid.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}     
	@${RM} dist/${CND_CONF}/${IMAGE_TYPE}/ninhid.X.${IMAGE_TYPE}.hex 
	
else
dist/${CND_CONF}/${IMAGE_TYPE}/ninhid.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk   
	@${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_CC} $(MP_EXTRA_LD_PRE) --chip=$(MP_PROCESSOR_OPTION) -G -mdist/${CND_CONF}/${IMAGE_TYPE}/ninhid.X.${IMAGE_TYPE}.map  --double=24 --float=24 --emi=wordwrite --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=require --mode=pro -DDEBUG -P -N100 -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --output=default,-inhx032 --runtime=default,+clear,-init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"     -odist/${CND_CONF}/${IMAGE_TYPE}/ninhid.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}     
	
endif


# Subprojects
.build-subprojects:


# Subprojects
.clean-subprojects:

# Clean Targets
.clean-conf: ${CLEAN_SUBPROJECTS}
	${RM} -r build/debug
	${RM} -r dist/debug

# Enable dependency checking
.dep.inc: .depcheck-impl

DEPFILES=$(shell mplabwildcard ${POSSIBLE_DEPFILES})
ifneq (${DEPFILES},)
include ${DEPFILES}
endif
