from datetime import datetime

from django.core.mail import EmailMessage
from django.http import HttpResponse, HttpResponseRedirect
from django.shortcuts import render
from django.template import RequestContext
from django.views.decorators.csrf import csrf_exempt

from .models import *


def index(request):
	return render(request, 'tool/index.xml', {})


def version_check(request):
	latest = ProgramVersion.objects.all().order_by('-version')[0]
	template_vars = {
		'version': latest.version,
		'release_date': latest.release_date,
		'release_notes': latest.release_notes,
		'url': latest.file.url,
	}

	return render(request, 'tool/version_check.xml', template_vars, content_type="text/xml")


def get_latest(request):
	latest = ProgramVersion.objects.all().order_by('-version')[0]
	return HttpResponseRedirect(latest.file.url)


def validate_mail_address(email):
	from django.core.validators import validate_email
	from django.core.exceptions import ValidationError
	try:
		validate_email(email)
		return True
	except ValidationError:
		return False


@csrf_exempt
def report_bug(request):
	if request.method == 'GET':
		return HttpResponse('expected POST')

	if 'HTTP_X_FORWARDED_FOR' in request.META:
		ip = request.META['HTTP_X_FORWARDED_FOR'].split(',')[0]
	elif 'HTTP_X_REAL_IP' in request.META:
		ip = request.META['HTTP_X_REAL_IP']
	else:
		ip = request.META['REMOTE_ADDR']
	subj = '[MUNIA] Bug report generated from ip %s' % (ip)

	message = 'MUNIA Bug Report Notification\r\n'
	message += '\r\n'
	message += 'A new bug report was generated at ' + str(datetime.now().replace(microsecond=0))
	message += '\r\n'

	message += 'Skin name: ' + request.POST['skin_name'] + '\r\n'
	message += 'Commandline used: ' + request.POST['command_line'] + '\r\n'
	message += 'Program version: ' + request.POST['program_version'] + '\r\n'
	if 'exception' in request.POST:
		message += 'Exception: ' + request.POST['exception'] + '\r\n'
	message += '\r\n'

	message += '\r\n'
	message += '\r\nThis mail was sent automatically by the portal @ MUNIA.io'

	reply_to = []
	cc = []
	if 'email' in request.POST:
		submitter = request.POST['email']
		if validate_mail_address(submitter):
			cc.append(submitter)
			reply_to.append(submitter)
		else:
			reply_to.append('noreply@munia.io')

	email = EmailMessage(subject=subj, body=message, from_email='bugs@munia.io', to=['frank@zzattack.org'], cc=cc,
		reply_to=reply_to)
	email.attach(request.POST['skin_name'] + '.svg', request.POST['skin_svg'], 'text/plain')

	email.send(fail_silently=False)

	return HttpResponse('OK')
