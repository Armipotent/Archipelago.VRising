#!/bin/sh

# This is a shorthand script for me to build
# and test the app on linux. I'll write a
# windows script someday(tm) - Armipotent

# Modify vrising_install_folder to your
# installation folder. DO NOT FORGET
# TO INSTALL BEPINEX, VCF, AND SERVER
# LAUNCH FIX.

declare vrising_install_folder="$HOME/.local/share/Steam/steamapps/common/VRising"

cd ./APVRising
dotnet build
cp ./obj/Debug/APVRising.dll ${vrising_install_folder}/BepInEx/plugins/
steam steam://rungameid/1604030