from django.shortcuts import render


def index(request):
    return render(request, 'home/index.html', {})


def contact(request):
    return render(request, 'home/contact.html', {})


def devices(request):
    return render(request, 'home/devices.html', {})


def faq(request):
    return render(request, 'home/faq.html', {})

def success(request):
    return render(request, 'home/success.html', {})


def software(request):
    return render(request, 'home/software.html', {})
