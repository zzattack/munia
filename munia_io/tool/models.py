from django.db import models
  
class ProgramVersion(models.Model):
    version = models.CharField(max_length=100, unique=True)
    release_date = models.DateField(auto_now_add=True)
    release_notes = models.CharField(max_length=100, unique=True)
    file = models.FileField(upload_to='versions')
    
    def __unicode__ (self):
        return 'Version ' + self.version.__str__()

    class Admin: pass
