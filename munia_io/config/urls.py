from django.conf import settings
from django.urls import *
from django.conf.urls.static import static
from django.contrib import admin

import home.urls
import tool.urls
import tool.views

urlpatterns = [
	re_path(r'^tool/', include(tool.urls)),
	re_path(r'^download', tool.views.get_latest),
	re_path(r'^', include(home.urls)),
	re_path(r'admin/', admin.site.urls),
] + static(settings.STATIC_URL, document_root=settings.STATIC_ROOT)
