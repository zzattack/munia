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
FINAL_IMAGE=dist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
else
IMAGE_TYPE=production
OUTPUT_SUFFIX=hex
DEBUGGABLE_SUFFIX=elf
FINAL_IMAGE=dist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}
endif

# Object Directory
OBJECTDIR=build/${CND_CONF}/${IMAGE_TYPE}

# Distribution Directory
DISTDIR=dist/${CND_CONF}/${IMAGE_TYPE}

# Source Files Quoted if spaced
SOURCEFILES_QUOTED_IF_SPACED=main.c usb_descriptors.c lcd.c usb_callbacks.c snes.c n64.c gamecube.c menu.c buttonchecker.c circular_buffer.c gamepad.c usb/usb_device.c usb/usb_device_hid.c

# Object Files Quoted if spaced
OBJECTFILES_QUOTED_IF_SPACED=${OBJECTDIR}/main.p1 ${OBJECTDIR}/usb_descriptors.p1 ${OBJECTDIR}/lcd.p1 ${OBJECTDIR}/usb_callbacks.p1 ${OBJECTDIR}/snes.p1 ${OBJECTDIR}/n64.p1 ${OBJECTDIR}/gamecube.p1 ${OBJECTDIR}/menu.p1 ${OBJECTDIR}/buttonchecker.p1 ${OBJECTDIR}/circular_buffer.p1 ${OBJECTDIR}/gamepad.p1 ${OBJECTDIR}/usb/usb_device.p1 ${OBJECTDIR}/usb/usb_device_hid.p1
POSSIBLE_DEPFILES=${OBJECTDIR}/main.p1.d ${OBJECTDIR}/usb_descriptors.p1.d ${OBJECTDIR}/lcd.p1.d ${OBJECTDIR}/usb_callbacks.p1.d ${OBJECTDIR}/snes.p1.d ${OBJECTDIR}/n64.p1.d ${OBJECTDIR}/gamecube.p1.d ${OBJECTDIR}/menu.p1.d ${OBJECTDIR}/buttonchecker.p1.d ${OBJECTDIR}/circular_buffer.p1.d ${OBJECTDIR}/gamepad.p1.d ${OBJECTDIR}/usb/usb_device.p1.d ${OBJECTDIR}/usb/usb_device_hid.p1.d

# Object Files
OBJECTFILES=${OBJECTDIR}/main.p1 ${OBJECTDIR}/usb_descriptors.p1 ${OBJECTDIR}/lcd.p1 ${OBJECTDIR}/usb_callbacks.p1 ${OBJECTDIR}/snes.p1 ${OBJECTDIR}/n64.p1 ${OBJECTDIR}/gamecube.p1 ${OBJECTDIR}/menu.p1 ${OBJECTDIR}/buttonchecker.p1 ${OBJECTDIR}/circular_buffer.p1 ${OBJECTDIR}/gamepad.p1 ${OBJECTDIR}/usb/usb_device.p1 ${OBJECTDIR}/usb/usb_device_hid.p1

# Source Files
SOURCEFILES=main.c usb_descriptors.c lcd.c usb_callbacks.c snes.c n64.c gamecube.c menu.c buttonchecker.c circular_buffer.c gamepad.c usb/usb_device.c usb/usb_device_hid.c


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
	${MAKE}  -f nbproject/Makefile-debug.mk dist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}

MP_PROCESSOR_OPTION=18F24K50
# ------------------------------------------------------------------------------------
# Rules for buildStep: compile
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
${OBJECTDIR}/main.p1: main.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/main.p1.d 
	@${RM} ${OBJECTDIR}/main.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/main.p1  main.c 
	@-${MV} ${OBJECTDIR}/main.d ${OBJECTDIR}/main.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/main.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb_descriptors.p1: usb_descriptors.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/usb_descriptors.p1.d 
	@${RM} ${OBJECTDIR}/usb_descriptors.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb_descriptors.p1  usb_descriptors.c 
	@-${MV} ${OBJECTDIR}/usb_descriptors.d ${OBJECTDIR}/usb_descriptors.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb_descriptors.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/lcd.p1: lcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/lcd.p1.d 
	@${RM} ${OBJECTDIR}/lcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/lcd.p1  lcd.c 
	@-${MV} ${OBJECTDIR}/lcd.d ${OBJECTDIR}/lcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/lcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb_callbacks.p1: usb_callbacks.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/usb_callbacks.p1.d 
	@${RM} ${OBJECTDIR}/usb_callbacks.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb_callbacks.p1  usb_callbacks.c 
	@-${MV} ${OBJECTDIR}/usb_callbacks.d ${OBJECTDIR}/usb_callbacks.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb_callbacks.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/snes.p1: snes.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/snes.p1.d 
	@${RM} ${OBJECTDIR}/snes.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/snes.p1  snes.c 
	@-${MV} ${OBJECTDIR}/snes.d ${OBJECTDIR}/snes.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/snes.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/n64.p1: n64.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/n64.p1.d 
	@${RM} ${OBJECTDIR}/n64.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/n64.p1  n64.c 
	@-${MV} ${OBJECTDIR}/n64.d ${OBJECTDIR}/n64.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/n64.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/gamecube.p1: gamecube.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/gamecube.p1.d 
	@${RM} ${OBJECTDIR}/gamecube.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/gamecube.p1  gamecube.c 
	@-${MV} ${OBJECTDIR}/gamecube.d ${OBJECTDIR}/gamecube.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/gamecube.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/menu.p1: menu.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/menu.p1.d 
	@${RM} ${OBJECTDIR}/menu.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/menu.p1  menu.c 
	@-${MV} ${OBJECTDIR}/menu.d ${OBJECTDIR}/menu.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/menu.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/buttonchecker.p1: buttonchecker.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/buttonchecker.p1.d 
	@${RM} ${OBJECTDIR}/buttonchecker.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/buttonchecker.p1  buttonchecker.c 
	@-${MV} ${OBJECTDIR}/buttonchecker.d ${OBJECTDIR}/buttonchecker.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/buttonchecker.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/circular_buffer.p1: circular_buffer.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/circular_buffer.p1.d 
	@${RM} ${OBJECTDIR}/circular_buffer.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/circular_buffer.p1  circular_buffer.c 
	@-${MV} ${OBJECTDIR}/circular_buffer.d ${OBJECTDIR}/circular_buffer.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/circular_buffer.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/gamepad.p1: gamepad.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/gamepad.p1.d 
	@${RM} ${OBJECTDIR}/gamepad.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/gamepad.p1  gamepad.c 
	@-${MV} ${OBJECTDIR}/gamepad.d ${OBJECTDIR}/gamepad.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/gamepad.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb/usb_device.p1: usb/usb_device.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/usb" 
	@${RM} ${OBJECTDIR}/usb/usb_device.p1.d 
	@${RM} ${OBJECTDIR}/usb/usb_device.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb/usb_device.p1  usb/usb_device.c 
	@-${MV} ${OBJECTDIR}/usb/usb_device.d ${OBJECTDIR}/usb/usb_device.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb/usb_device.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb/usb_device_hid.p1: usb/usb_device_hid.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/usb" 
	@${RM} ${OBJECTDIR}/usb/usb_device_hid.p1.d 
	@${RM} ${OBJECTDIR}/usb/usb_device_hid.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb/usb_device_hid.p1  usb/usb_device_hid.c 
	@-${MV} ${OBJECTDIR}/usb/usb_device_hid.d ${OBJECTDIR}/usb/usb_device_hid.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb/usb_device_hid.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
else
${OBJECTDIR}/main.p1: main.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/main.p1.d 
	@${RM} ${OBJECTDIR}/main.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/main.p1  main.c 
	@-${MV} ${OBJECTDIR}/main.d ${OBJECTDIR}/main.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/main.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb_descriptors.p1: usb_descriptors.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/usb_descriptors.p1.d 
	@${RM} ${OBJECTDIR}/usb_descriptors.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb_descriptors.p1  usb_descriptors.c 
	@-${MV} ${OBJECTDIR}/usb_descriptors.d ${OBJECTDIR}/usb_descriptors.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb_descriptors.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/lcd.p1: lcd.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/lcd.p1.d 
	@${RM} ${OBJECTDIR}/lcd.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/lcd.p1  lcd.c 
	@-${MV} ${OBJECTDIR}/lcd.d ${OBJECTDIR}/lcd.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/lcd.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb_callbacks.p1: usb_callbacks.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/usb_callbacks.p1.d 
	@${RM} ${OBJECTDIR}/usb_callbacks.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb_callbacks.p1  usb_callbacks.c 
	@-${MV} ${OBJECTDIR}/usb_callbacks.d ${OBJECTDIR}/usb_callbacks.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb_callbacks.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/snes.p1: snes.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/snes.p1.d 
	@${RM} ${OBJECTDIR}/snes.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/snes.p1  snes.c 
	@-${MV} ${OBJECTDIR}/snes.d ${OBJECTDIR}/snes.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/snes.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/n64.p1: n64.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/n64.p1.d 
	@${RM} ${OBJECTDIR}/n64.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/n64.p1  n64.c 
	@-${MV} ${OBJECTDIR}/n64.d ${OBJECTDIR}/n64.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/n64.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/gamecube.p1: gamecube.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/gamecube.p1.d 
	@${RM} ${OBJECTDIR}/gamecube.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/gamecube.p1  gamecube.c 
	@-${MV} ${OBJECTDIR}/gamecube.d ${OBJECTDIR}/gamecube.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/gamecube.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/menu.p1: menu.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/menu.p1.d 
	@${RM} ${OBJECTDIR}/menu.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/menu.p1  menu.c 
	@-${MV} ${OBJECTDIR}/menu.d ${OBJECTDIR}/menu.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/menu.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/buttonchecker.p1: buttonchecker.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/buttonchecker.p1.d 
	@${RM} ${OBJECTDIR}/buttonchecker.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/buttonchecker.p1  buttonchecker.c 
	@-${MV} ${OBJECTDIR}/buttonchecker.d ${OBJECTDIR}/buttonchecker.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/buttonchecker.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/circular_buffer.p1: circular_buffer.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/circular_buffer.p1.d 
	@${RM} ${OBJECTDIR}/circular_buffer.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/circular_buffer.p1  circular_buffer.c 
	@-${MV} ${OBJECTDIR}/circular_buffer.d ${OBJECTDIR}/circular_buffer.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/circular_buffer.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/gamepad.p1: gamepad.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}" 
	@${RM} ${OBJECTDIR}/gamepad.p1.d 
	@${RM} ${OBJECTDIR}/gamepad.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/gamepad.p1  gamepad.c 
	@-${MV} ${OBJECTDIR}/gamepad.d ${OBJECTDIR}/gamepad.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/gamepad.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb/usb_device.p1: usb/usb_device.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/usb" 
	@${RM} ${OBJECTDIR}/usb/usb_device.p1.d 
	@${RM} ${OBJECTDIR}/usb/usb_device.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb/usb_device.p1  usb/usb_device.c 
	@-${MV} ${OBJECTDIR}/usb/usb_device.d ${OBJECTDIR}/usb/usb_device.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb/usb_device.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
${OBJECTDIR}/usb/usb_device_hid.p1: usb/usb_device_hid.c  nbproject/Makefile-${CND_CONF}.mk
	@${MKDIR} "${OBJECTDIR}/usb" 
	@${RM} ${OBJECTDIR}/usb/usb_device_hid.p1.d 
	@${RM} ${OBJECTDIR}/usb/usb_device_hid.p1 
	${MP_CC} --pass1 $(MP_EXTRA_CC_PRE) --chip=$(MP_PROCESSOR_OPTION) -Q -G  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    -o${OBJECTDIR}/usb/usb_device_hid.p1  usb/usb_device_hid.c 
	@-${MV} ${OBJECTDIR}/usb/usb_device_hid.d ${OBJECTDIR}/usb/usb_device_hid.p1.d 
	@${FIXDEPS} ${OBJECTDIR}/usb/usb_device_hid.p1.d $(SILENT) -rsi ${MP_CC_DIR}../  
	
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: assemble
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
else
endif

# ------------------------------------------------------------------------------------
# Rules for buildStep: link
ifeq ($(TYPE_IMAGE), DEBUG_RUN)
dist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk    
	@${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_CC} $(MP_EXTRA_LD_PRE) --chip=$(MP_PROCESSOR_OPTION) -G -mdist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.map  -D__DEBUG=1 --debugger=pickit3  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"       --memorysummary dist/${CND_CONF}/${IMAGE_TYPE}/memoryfile.xml -odist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}     
	@${RM} dist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.hex 
	
else
dist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.${OUTPUT_SUFFIX}: ${OBJECTFILES}  nbproject/Makefile-${CND_CONF}.mk    bootloader.hex
	@${MKDIR} dist/${CND_CONF}/${IMAGE_TYPE} 
	${MP_CC} $(MP_EXTRA_LD_PRE) --chip=$(MP_PROCESSOR_OPTION) -G -mdist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.map  --double=24 --float=24 --emi=wordwrite --rom=-0-FFF,-1006-1007,-1016-1017 --opt=default,+asm,+asmfile,+speed,-space,-debug --addrqual=ignore --mode=pro -P -N255 -I"./USB" -I"." --warn=0 --asmlist --summary=default,-psect,-class,+mem,-hex,-file --codeoffset=0x1000 --output=default,-inhx032 --runtime=default,+clear,+init,-keep,-no_startup,-download,+config,+clib,+plib --output=-mcof,+elf:multilocs --stack=compiled:auto:auto:auto "--errformat=%f:%l: error: (%n) %s" "--warnformat=%f:%l: warning: (%n) %s" "--msgformat=%f:%l: advisory: (%n) %s"    --memorysummary dist/${CND_CONF}/${IMAGE_TYPE}/memoryfile.xml -odist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.${DEBUGGABLE_SUFFIX}  ${OBJECTFILES_QUOTED_IF_SPACED}     
	
	@echo "Creating unified hex file"
	@"C:/Program Files (x86)/Microchip/MPLABX/v3.05/mplab_ide/mplab_ide/modules/../../bin/hexmate" --edf="C:/Program Files (x86)/Microchip/MPLABX/v3.05/mplab_ide/mplab_ide/modules/../../dat/en_msgs.txt" dist/${CND_CONF}/${IMAGE_TYPE}/munia.X.${IMAGE_TYPE}.hex bootloader.hex -odist/${CND_CONF}/production/munia.X.production.unified.hex

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