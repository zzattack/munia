from django.db import models


class ProgramVersion(models.Model):
    version = models.CharField(max_length=100, unique=True)
    release_date = models.DateField(auto_now_add=True)
    release_notes = models.CharField(max_length=100)
    file = models.FileField(upload_to='versions')

    def __unicode__ (self):
        return 'Version ' + self.version.__str__()

    class Admin: pass


class InputAdapterDevice(models.Model):
    name = models.CharField(max_length=100, unique=True)


class FirmwareVersion(models.Model):
    version = models.CharField(max_length=100, unique=True)
    release_date = models.DateField(auto_now_add=True)
    release_notes = models.CharField(max_length=100)
    file = models.FileField(upload_to='versions')

    device = models.ForeignKey(InputAdapterDevice, on_delete=models.CASCADE)
    supported_hw_revisions = models.CharField(max_length=100) # range for device hardware revision
    supported_device_ids = models.CharField(max_length=100) # regex for microcontroller device id

    def __unicode__ (self):
        return 'FW Version ' + self.version.__str__()

    class Admin: pass