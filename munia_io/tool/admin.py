from django.contrib import admin
from .models import *

@admin.register(ProgramVersion)
class ProgramVersionAdmin(admin.ModelAdmin): pass

@admin.register(FirmwareVersion)
class FirmwareVersionAdmin(admin.ModelAdmin): pass

