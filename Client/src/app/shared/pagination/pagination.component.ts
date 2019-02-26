import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

@Component({
  selector: 'app-pagination',
  templateUrl: './pagination.component.html',
  styleUrls: ['./pagination.component.css']
})
export class PaginationComponent implements OnInit {
  @Input() totalItems: number;
  @Input() pageSize: number;
  @Output() pageChanged = new EventEmitter();
  pages: any[];
  currentPage: number;
  constructor() { }

  ngOnInit() {
    this.currentPage = 1;
    const pagesCount = Math.ceil(this.totalItems / this.pageSize);
    this.pages = [];
    for (let i = 1; i <= pagesCount; i++) {
      this.pages.push(i);
    }
  }
  changePage(page) {
    this.currentPage = page;
    this.pageChanged.emit(page);
  }
  previous() {
    if (this.currentPage === 1) {
      return;
    }
    this.currentPage--;
    this.pageChanged.emit(this.currentPage);
  }
  next() {
    if (this.currentPage === Number(this.pages.length)) {
      return;
    }
    this.currentPage++;
    this.pageChanged.emit(this.currentPage);
  }
}
