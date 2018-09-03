import { Component, OnInit, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.less']
})

export class HeaderComponent implements OnInit {
  title = 'Scheduler';
  isNavbarCollapsed = false;

  constructor(private router: Router, private route: ActivatedRoute) {

  }

  ngOnInit(): void {  }
}
