from datetime import datetime

from django.core.mail import EmailMessage
from django.shortcuts import render
from django.views.decorators.csrf import csrf_protect


def index(request):
	return render(request, 'home/index.html', {})


def validate_mail_address(email):
	from django.core.validators import validate_email
	from django.core.exceptions import ValidationError
	try:
		validate_email(email)
		return True
	except ValidationError:
		return False


@csrf_protect
def contact(request):
	if request.method == "GET":
		return render(request, 'home/contact.html', {})

	success = False
	try:
		if 'HTTP_X_FORWARDED_FOR' in request.META:
			ip = request.META['HTTP_X_FORWARDED_FOR'].split(',')[0]
		elif 'HTTP_X_REAL_IP' in request.META:
			ip = request.META['HTTP_X_REAL_IP']
		else:
			ip = request.META['REMOTE_ADDR']
		subj = '[MUNIA] Contact form notification from ip %s' % (ip)

		message = 'MUNIA Contact form notification\r\n'
		message += '\r\n'
		message += 'Contact form was filled out at ' + str(datetime.now().replace(microsecond=0))
		message += '\r\n'

		message += 'Submitter: ' + request.POST['name'] + '\r\n'
		message += 'Email: ' + request.POST['email'] + '\r\n'
		message += 'Message: \r\n----------------------------------------\r\n' + request.POST[
			'message'] + '\r\n----------------------------------------\r\n'

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

		email = EmailMessage(subject=subj, body=message, from_email='info@munia.io', to=['frank@zzattack.org'], cc=cc,
			reply_to=reply_to)

		email.send(fail_silently=False)
		success = True

	except: pass

	return render(request, 'home/contact_response.html', {'contact_success': success})


def devices(request):
	return render(request, 'home/devices.html', {})


def faq(request):
	return render(request, 'home/faq.html', {})


def success(request):
	return render(request, 'home/success.html', {})


def software(request):
	return render(request, 'home/software.html', {})
