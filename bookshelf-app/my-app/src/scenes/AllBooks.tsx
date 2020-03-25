import { BookCard } from "../components/BookCard";
import React, { useState, useEffect } from "react";
import { getAllBooks, loanBook } from "../_services/api.service";
import { Book } from "../_models/Book";
import "./AllBooks.css"

export function AllBooks() {
    const [books, setBooks] = useState<Book[]>([]);

    let array = [];
    for (let i = 0; i < books.length; i++) {
        array.push(
            <BookCard key={i} book={books[i]} onClick={loanBook} />
        );
    }

    useEffect(() => {
        getAllBooks().then(books => {
            setBooks(books as Book[]);
        });
    }, []);

    return (
        <div className="All-books">
            {array}
        </div>
    );
}